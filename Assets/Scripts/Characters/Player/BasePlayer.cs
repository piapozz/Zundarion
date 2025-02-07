using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerAnimation;

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
    public MoveAnimation selfMoveState  { get; set; } = MoveAnimation.IDLE;

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
    private PlayerMove _selfMove ;

    /// <summary>プレイヤーの攻撃コンポーネント</summary>
    private PlayerAttack _selfAttack;

    /// <summary>プレイヤーのパリィコンポーネント</summary>
    private PlayerParry _selfParry;

    /// <summary>プレイヤーの入力</summary>
    private PlayerInput _playerInput ;

    /// <summary>プレイヤーの移動入力状態</summary>
    private InputAction _inputAction;

    /// <summary>長押しを受け取る対象のAction</summary>
    private InputActionReference _hold;

    /// <summary>ターゲットの前フレームでの座標</summary>
    private Vector3 _oldPosition = Vector3.zero;

    /// <summary>操作可能かどうか</summary>
    private bool _operable = true;

    /// <summary>入力された移動方向</summary>
    private Vector2 _inputMoveDir = Vector2.zero;

    /// <summary>移動に掛かる倍率</summary>
    private float _currentMultiplier = 1.0f;

    private const float _RUN_SPEED_RATE = 1.5f;
    private const float _AVOID_SPEED_RATE = 2.0f;

    void Awake()
    {
        obstacleLayerMask = LayerMask.GetMask("FieldObject");
        _playerInput = GetComponent<PlayerInput>();
        _inputAction = _playerInput.actions["Move"];

        Init();

        if (_hold == null) return;

        // InputActionReferenceのholdにハンドラを登録する
        _hold.action.performed += OnRun;

        // 入力を受け取るために有効化
        _hold.action.Enable();
    }

    private void Update()
    {

        Debug.Log(selfMoveState);
        MoveExecute();

        /*
        // ターゲットの情報収集
        this.transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if(inputRun) 
            selfMoveState = PlayerAnimation.MoveAnimation.AVOIDANCE;
        else if(selfMoveState != PlayerAnimation.MoveAnimation.RUN) 
            selfMoveState = PlayerAnimation.MoveAnimation.WALK;

        if(inputMove == Vector2.zero && selfMoveState != PlayerAnimation.MoveAnimation.AVOIDANCE) 
            selfMoveState = PlayerAnimation.MoveAnimation.IDLE;

        _selfMove.Move(inputMove, selfMoveState);

        if(inputAttack)
            _selfAttack.Attack();

        if (inputMove == Vector2.zero) 
            return;

        // 出力の調整
        Quaternion q = Quaternion.AngleAxis(selfFrontAngleZ - 90, Vector3.down);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation,  q, 1.5f);
        */
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

        UniTask task = RunExecute();
    }

    /// <summary>
    /// ActionsのAttakに割り当てられている入力があったなら実行
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() != 0) inputAttack = true;
        else inputAttack = false;
    }

    /// <summary>
    /// ActionsのParryに割り当てられている入力があったなら実行
    /// </summary>
    /// <param name="context"></param>
    public void OnParry(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        _selfParry.Parry();
    }

    public override bool IsPlayer() { return true; }

    public override void TakeDamage(float damageSize)
    {
        base.TakeDamage(damageSize);
        if (health <= 0)
            selfAnimator.SetTrigger(selfAnimationData.anyStatePram[(int)AnyStateAnimation.DIE]);
    }

    /// <summary>
    /// 移動実行処理
    /// </summary>
    private void MoveExecute()
    {
        if (!_operable) return;
        if (_inputMoveDir.x == 0 && _inputMoveDir.y == 0)
        {
            // アニメーション設定
            selfMoveState = MoveAnimation.IDLE;
            _currentMultiplier = 0.0f;
        }

        else
        {
            selfMoveState = MoveAnimation.WALK;
            _currentMultiplier = 1.0f;
        }

        // 回転し移動
        Rotate(AdjustMoveDir());

        // アニメーションを変更
        selfAnimator.SetInteger("Move", (int)selfMoveState);

        Move(speed * _currentMultiplier);
    }

    /// <summary>
    /// 走りアクションを実行
    /// </summary>
    /// <returns></returns>
    private async UniTask RunExecute()
    {
        if (!_operable) return;

        SetAvoidState();
        // 回避が終わるまで待機
        while (CheckAnimation(selfAnimationData.anyStatePram[(int)AnyStateAnimation.AVOID]))
        {
            await UniTask.DelayFrame(1);
        }
        SetRunState();
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
        selfAnimator.SetTrigger("Avoidance");
        _currentMultiplier = _AVOID_SPEED_RATE;
    }

    private void SetIdleState()
    {

    }

    private void SetRunState()
    {
        selfMoveState = MoveAnimation.RUN;
        _currentMultiplier = _RUN_SPEED_RATE;
    }

    public void OperableEvent(bool setOperable)
    {
        _operable = setOperable;
    }

    private bool CheckAnimation(string animationName)
    {
        return selfAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
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
