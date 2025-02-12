using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

using static PlayerAnimation;
using static CommonModule;
using System.Threading;

public abstract class BasePlayer : BaseCharacter
{
    // public //////////////////////////////////////////////////////////////////

    /// <summary>プレイヤーの初期設定</summary>
    public virtual void Setup()
    {

    }

    /// <summary>自身の現在の体力</summary>
    public float selfCurrentHealth { get; protected set; }

    /// <summary>攻撃コンボの最大数</summary>
    public int selfComboCountMax { get; protected set; }

    /// <summary>攻撃コンボ数</summary>
    public int selfComboCount { get; protected set; }

    /// <summary>現在の移動ステート</summary>
    public MoveAnimation selfMoveState { get; set; } = MoveAnimation.IDLE;

    /// <summary>アニメーションの再生速度</summary>
    public float selfAnimationSpeed { get; protected set; }

    /// <summary>アニメーションのパラメーター情報</summary>
    public PlayerAnimation selfAnimationData { get; protected set; }

    /// <summary>自身の前方アングル</summary>
    public float selfFrontAngleZ { get; set; }

    /// <summary>自身のゲームオブジェクト</summary>
    public GameObject selfGameObject { get; private set; }

    // protected //////////////////////////////////////////////////////////////////

    /// <summary>派生先による初期化</summary>
    protected abstract void Init();

    /// <summary>障害物のレイヤーマスク</summary>
    protected LayerMask obstacleLayerMask { get; private set; }

    /// <summary>プレイヤーの入力方向</summary>
    protected Vector2 inputMove = Vector3.zero;

    /// <summary>走る入力</summary>
    protected bool inputRun = false;

    /// <summary>攻撃入力</summary>
    protected bool inputAttack = false;

    /// <summary>パリィ入力</summary>
    protected bool inputParry = false;

    // private //////////////////////////////////////////////////////////////////

    /// <summary>プレイヤーの移動コンポーネント</summary>
    private PlayerMove _selfMove;

    /// <summary>プレイヤーの攻撃コンポーネント</summary>
    private PlayerAttack _selfAttack;

    /// <summary>プレイヤーのパリィコンポーネント</summary>
    private PlayerParry _selfParry;

    /// <summary>プレイヤーの入力</summary>
    private PlayerInput _playerInput;

    /// <summary>プレイヤーの移動入力状態</summary>
    private InputAction _inputAction;

    /// <summary>長押しを受け取る対象のAction</summary>
    private InputActionReference _hold;

    /// <summary>ターゲットの前フレームでの座標</summary>
    private Vector3 _oldPosition = Vector3.zero;

    /// <summary>入力された移動方向</summary>
    private Vector2 _inputMoveDir = Vector2.zero;

    /// <summary>移動に掛かる倍率</summary>
    private float _currentMultiplier = 1.0f;

    /// <summary>移動硬直中かどうか</summary>
    private bool _isMoveStiff = false;

    /// <summary>硬直中かどうか</summary>
    private bool _isAllStiff = false;

    /// <summary>プレイヤーの先行入力情報</summary>
    private PreInput _selfPreInput = null;

    /// <summary>パリィのストック</summary>
    private int _parryStock = 0;

    /// <summary>回避のストック</summary>
    private int _avoidStock = 0;

    /// <summary>パリィのクールダウンタスク</summary>
    private UniTask _parryCoolDownTask;

    private CancellationTokenSource _parryCTS = null;

    /// <summary>回避のクールダウンタスク</summary>
    private UniTask _avoidCoolDownTask;

    private CancellationTokenSource _avoidCTS = null;

    private const float _RUN_SPEED_RATE = 1.25f;
    private const float _AVOID_SPEED_RATE = 2.0f;
    private const float _ATTACK_SENS_RANGE = 10.0f;
    private const int _PARRY_COOL_DOWN_STOCK = 2;
    private const float _PARRY_COOL_DOWN_SECOND = 2.0f;
    private const int _AVOID_COOL_DOWN_STOCK = 2;
    private const float _AVOID_COOL_DOWN_SECOND = 2.0f;

    void Awake()
    {
        obstacleLayerMask = LayerMask.GetMask("FieldObject");
        _playerInput = GetComponent<PlayerInput>();
        _inputAction = _playerInput.actions["Move"];
        _selfPreInput = GetComponent<PreInput>();
        _selfPreInput.Initialize();

        _parryStock = _PARRY_COOL_DOWN_STOCK;
        _avoidStock = _AVOID_COOL_DOWN_STOCK;

        Init();
    }

    private void Update()
    {
        MoveExecute();
    }

    /// <summary>
    /// アクションマップのMoveに登録されているキーが押されたときに入力値を取得
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        _inputMoveDir = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// ActionsのRunに割り当てられている入力があったなら実行
    /// </summary>
    /// <param name="context"></param>
    public void OnRun(InputAction.CallbackContext context)
    {
        if (!context.performed || _isAllStiff) return;
        UniTask task = RunExecute();
    }

    /// <summary>
    /// ActionsのAttakに割り当てられている入力があったなら実行
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || _isAllStiff) return;
        Attack();
    }

    /// <summary>
    /// ActionsのParryに割り当てられている入力があったなら実行
    /// </summary>
    /// <param name="context"></param>
    public void OnParry(InputAction.CallbackContext context)
    {
        if (!context.performed || _isAllStiff) return;
        Parry();
    }

    /// <summary>
    /// 移動実行処理
    /// </summary>
    private void MoveExecute()
    {
        // 移動できないなら処理を抜ける
        if (_isAllStiff || _isMoveStiff) return;
        // 移動方向が入力されていないなら処理を抜ける
        if (_inputMoveDir.x == 0 && _inputMoveDir.y == 0)
        {
            selfAnimator.SetBool("Move", false);
            return;
        }
        selfAnimator.SetBool("Move", true);

        if (selfMoveState == MoveAnimation.IDLE)
            SetWalkState();

        // 回転し移動
        Rotate(AdjustMoveDir());
        Move(speed * _currentMultiplier);
    }

    /// <summary>
    /// 走りアクションを実行
    /// </summary>
    /// <returns></returns>
    private async UniTask RunExecute()
    {
        if (_isAllStiff) return;

        // クールダウン中なら処理を抜ける
        if (CheckAvoidCoolDown()) return;

        SetAvoidState();
        while (!CheckAnimation(selfAnimationData.anyStatePram[(int)AnyStateAnimation.AVOID]))
        {
            await UniTask.DelayFrame(1);
        }
        // 回避が終わるまで待機
        while (CheckAnimation(selfAnimationData.anyStatePram[(int)AnyStateAnimation.AVOID]))
        {
            Move(speed * _currentMultiplier);
            await UniTask.DelayFrame(1);
        }
        SetRunState();
    }

    private bool CheckAvoidCoolDown()
    {
        if (_avoidStock <= 0) return true;

        _avoidStock--;
        _avoidCTS = new CancellationTokenSource();
        // クールダウン中ならキャンセル
        if (!_avoidCoolDownTask.Status.IsCompleted())
            _avoidCTS.Cancel();

        _avoidCoolDownTask = WaitAction(_AVOID_COOL_DOWN_SECOND, () => _avoidStock = _AVOID_COOL_DOWN_STOCK, _avoidCTS.Token);

        return false;
    }

    /// <summary>
    /// 入力情報から移動方向を調整
    /// </summary>
    /// <returns></returns>
    private Vector3 AdjustMoveDir()
    {
        // カメラの方向に基づいて入力ベクトルを修正
        Vector3 cameraForward = CameraManager.instance.selfCamera.transform.forward;
        Vector3 cameraRight = CameraManager.instance.selfCamera.transform.right;
        // カメラ方向に基づいた移動ベクトルを計算
        Vector3 adjustedMove = (cameraRight * _inputMoveDir.x + cameraForward * _inputMoveDir.y).normalized;
        return adjustedMove;
    }

    private void SetAvoidState()
    {
        selfAnimator.SetTrigger(selfAnimationData.anyStatePram[(int)AnyStateAnimation.AVOID]);
        _currentMultiplier = _AVOID_SPEED_RATE;
        _isMoveStiff = true;
    }

    private void SetWalkState()
    {
        _currentMultiplier = 1.0f;
    }

    private void SetRunState()
    {
        _currentMultiplier = _RUN_SPEED_RATE;
        _isMoveStiff = false;
    }

    /// <summary>
    /// 攻撃する
    /// </summary>
    public void Attack()
    {
        // アニメーション設定
        selfAnimator.SetTrigger(selfAnimationData.attackPram[(int)AttackAnimation.ATTACK]);
        // 敵の方向を向く
        BaseCharacter character = CharacterManager.instance.GetNearCharacter(this, _ATTACK_SENS_RANGE);
        if (character == null) return;
        TurnAround(character.gameObject.transform);
    }

    /// <summary>
    /// パリィする
    /// </summary>
    public void Parry()
    {
        // パリィになるか判定
        List<BaseCharacter> parryList = CollisionManager.instance.parryList;
        if (parryList.Count == 0) return;
        // パリィクールダウン中なら処理を抜ける
        if (CheckParryCoolDown()) return;
        // アニメーションをセット
        selfAnimator.SetTrigger(selfAnimationData.anyStatePram[(int)AnyStateAnimation.PARRY]);
        // パリィ相手のアニメーションをひるみにする
        parryList[0].selfAnimator.SetTrigger(selfAnimationData.anyStatePram[(int)AnyStateAnimation.IMPACT]);
        // プレイヤーを敵の方向に向ける
        TurnAround(parryList[0].transform);
        // 通常カメラをリセット
        CameraManager.instance.SetFreeCam(transform.eulerAngles.y, 0.5f);
    }

    /// <summary>
    /// パリィクールダウン処理
    /// </summary>
    /// <returns></returns>
    private bool CheckParryCoolDown()
    {
        if (_parryStock <= 0) return true;

        _parryStock--;
        _parryCTS = new CancellationTokenSource();
        // クールダウン中ならキャンセル
        if (!_parryCoolDownTask.Status.IsCompleted())
            _parryCTS.Cancel();

        _parryCoolDownTask = WaitAction(_PARRY_COOL_DOWN_SECOND, () => _parryStock = _PARRY_COOL_DOWN_STOCK, _parryCTS.Token);
        return false;
    }

    private bool CheckAnimation(string animationName)
    {
        return selfAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    public void SetAllStiffEvent(float second)
    {
        _isAllStiff = true;
        UniTask task = WaitAction(second, () => _isAllStiff = false);
    }

    public void SetMoveStiffEvent(float second)
    {
        _isMoveStiff = true;
        UniTask task = WaitAction(second, () => _isMoveStiff = false);
    }

    public override bool IsPlayer() { return true; }

    public override void TakeDamage(float damageSize)
    {
        base.TakeDamage(damageSize);
        if (health <= 0)
            selfAnimator.SetTrigger(selfAnimationData.anyStatePram[(int)AnyStateAnimation.DIE]);
    }

#if GUI_OUTPUT

    /// <summary>GUI出力用 インスタンスカウンタ</summary>
    static private int gui_instanceTotalNum = 0;

    /// <summary>GUI出力用 インスタンス番号</summary>
    private int gui_instanceNum;

    /// <summary>インスペクタ用GUI表示／非表示フラグ</summary>
    [SerializeField]
    private bool enableGUIOutput = true;

    /// <summary>
    /// 毎フレーム呼ばれるGUI出力用メソッド
    /// <para>
    /// 画面にデバッグ用の情報を出す
    /// （重いので実機には乗らないようにする）
    /// </para>
    /// </summary>
    private void OnGUI()
    {
        if (!enableGUIOutput)
        {
            return;
        }
        Color oldColor = GUI.color;
        GUI.color = Color.yellow;
        using (new GUILayout.AreaScope(new Rect(0, 0, Screen.width, Screen.height)))
        {
            using (new GUILayout.VerticalScope())
            {
                using (new GUILayout.HorizontalScope())
                {
                    if (gui_instanceNum == 0)
                    {
                        using (new GUILayout.VerticalScope("box"))
                        {
                            GUIOutputSelfInfo();
                            GUILayout.Space(20);
                        }
                    }
                    GUILayout.FlexibleSpace();
                    if (gui_instanceNum == 1)
                    {
                        using (new GUILayout.VerticalScope("box"))
                        {
                            GUIOutputSelfInfo();
                            GUILayout.Space(20);
                        }
                    }
                }
                GUILayout.FlexibleSpace();
                if (gui_instanceNum == 0)
                {

                }
            }
        }
        GUI.color = oldColor;
    }
#endif  // GUI_OUTPUT

    /// <summary>GUIに自身の情報を出力</summary>
    protected void GUIOutputSelfInfo()
    {
#if GUI_OUTPUT
        GUILayout.Label("SelfPosition: " + transform.position);
        GUILayout.Label("SelfYRotation: " + transform.rotation.eulerAngles.y);
        GUILayout.Label("input.x: " + _inputMove.z "input.y" + _inputMove.y);
#endif  // GUI_OUTPUT
    }
}
