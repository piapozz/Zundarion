using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;
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
    public PlayerAnimation.MoveAnimation selfMoveState  { get; set; }

    /// <summary>プレイヤーの移動速度</summary>
    public float selfMoveSpeed { get; protected set; }

    /// <summary>アニメーションの再生速度</summary>
    public float selfAnimationSpeed { get; protected set; }

    /// <summary>アニメーションのパラメーター情報</summary>
    public PlayerAnimation selfAnimationData { get; protected set; }

    /// <summary>当たり判定情報</summary>
    public TagData selfCollisionData { get; protected set; }

    /// <summary>当たり判定発生情報</summary>
    public OccurrenceFrame selfOccurrenceFrame { get; protected set; }

    /// <summary>自身の前方アングル</summary>
    public float selfFrontAngleZ { get; set; }

    /// <summary>自身のゲームオブジェクト</summary>
    public GameObject selfGameObject { get; private set; }

    /// <summary>プレイヤーの当たり判定チェック</summary>
    public CheckCollision selfCheckCollision { get; protected set; }

    // protected //////////////////////////////////////////////////////////////////

    /// <summary>派生先による初期化</summary>
    protected abstract void Init();

    /// <summary>毎フレーム呼ばれるAIによる操作</summary>
    // protected abstract void UpdateAI();

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
    private InputActionReference hold;

    /// <summary>戦車の砲弾発射コンポーネント</summary>
    //private TankShooting selfTankShooting = null;

    /// <summary>ターゲットの前フレームでの座標</summary>
    private Vector3 _oldPosition = Vector3.zero;

    // Start is called before the first frame update
    /// <summary>
    /// 開始時に１度呼ばれる
    /// </summary>
    void Awake()
    {
        obstacleLayerMask = LayerMask.GetMask("FieldObject");
        _playerInput = GetComponent<PlayerInput>();
        _inputAction = _playerInput.actions["Move"];
        _selfMove = GetComponent<PlayerMove>();
        _selfAttack = GetComponent<PlayerAttack>();
        _selfParry = GetComponent<PlayerParry>();
        selfMoveState = PlayerAnimation.MoveAnimation.IDLE;
        selfGameObject = this.gameObject;
        selfCheckCollision = GetComponent<CheckCollision>();

        Init();

        if (hold == null) return;

        // InputActionReferenceのholdにハンドラを登録する
        hold.action.performed += OnRun;

        // 入力を受け取るために有効化
        hold.action.Enable();
    }


    // Update is called once per frame
    /// <summary>
    /// ゲームループで（1秒間に何回も）呼ばれる
    /// </summary>
    private void Update()
    {
        // ターゲットの情報収集
        this.transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if(inputRun) 
            selfMoveState = PlayerAnimation.MoveAnimation.AVOIDANCE;
        else if(selfMoveState != PlayerAnimation.MoveAnimation.RUN) 
            selfMoveState = PlayerAnimation.MoveAnimation.WALK;

        if(inputMove == Vector2.zero && selfMoveState != PlayerAnimation.MoveAnimation.AVOIDANCE) 
            selfMoveState = PlayerAnimation.MoveAnimation.IDLE;

        // AIの更新
        // UpdateAI();

        _selfMove.Move(inputMove, selfMoveState);

        if(inputAttack)
            _selfAttack.Attack();

        if (inputMove == Vector2.zero) 
            return;

        // 出力の調整
        Quaternion q = Quaternion.AngleAxis(selfFrontAngleZ - 90, Vector3.down);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation,  q, 1.5f);

        // 次フレームのために情報を残す

    }

    /// <summary>
    /// プレイヤーにダメージを与える
    /// </summary>
    /*
    public void TakeDamage(float damage)
    {
        // 回避中ならダメージを食らわない
        // 0 レイヤーの再生されているアニメーション情報を呼び出す
        AnimatorStateInfo stateInfo = selfAnimator.GetCurrentAnimatorStateInfo(0);

        // 定期的にコンボ回数を初期化する
        if (stateInfo.IsName("Avoidance") || stateInfo.IsName("Parry")) return;

        selfAnimator.SetTrigger(selfAnimationData.interruptPram[(int)InterruqtAnimation.IMPACT]);
        selfCurrentHealth -= damage;
    }*/

    // アクションマップのMoveに登録されているキーが押されたときに入力値を取得
    public void OnMove(InputAction.CallbackContext context)
    {
        inputMove = context.ReadValue<Vector2>();
    }

    // ActionsのRunに割り当てられている入力があったなら実行
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() != 0) inputRun = true;
        else inputRun = false;
    }

    // ActionsのAttakに割り当てられている入力があったなら実行
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
