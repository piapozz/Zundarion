using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayer : MonoBehaviour
{
    // public //////////////////////////////////////////////////////////////////

    /// <summary>プレイヤーの初期設定</summary>
    public virtual void Setup()
    {
       
    }

    /// <summary>自身の現在の体力</summary>
    //public float selfCurrentHealth { get => selfTankHealth?.CurrentHealth ?? 0.0f; }

    /// <summary>攻撃コンボの最大数</summary>
    public float selfComboCount { get; protected set; }

    /// <summary>プレイヤーの移動速度</summary>
    public float selfMoveSpeed { get; protected set; }

    /// <summary>アニメーションの再生速度</summary>
    public float selfAnimationSpeed { get; protected set; }

    /// <summary>アニメーションのパラメーター情報</summary>
    public PlayerAnimation selfAnimationData { get; protected set; }

    /// <summary>アニメーションのパラメーター情報</summary>
    public float selfFrontAngle { get; set; }

    // protected //////////////////////////////////////////////////////////////////

    /// <summary>毎フレーム呼ばれるAIによる操作</summary>
    protected abstract void UpdateAI();

    /// <summary>障害物のレイヤーマスク</summary>
    protected LayerMask obstacleLayerMask { get; private set; }

    // private //////////////////////////////////////////////////////////////////

    /// <summary>アニメーターコンポーネント(戦車以外はnull)</summary>
    private Animator _selfAnimator = null;

    /// <summary>自身が戦車だった場合のTankHealthコンポーネント(戦車以外はnull)</summary>
    //private TankHealth selfTankHealth = null;

    /// <summary>戦車の移動コンポーネント</summary>
    //private TankMovement selfTankMovement = null;

    /// <summary>戦車の砲弾発射コンポーネント</summary>
    //private TankShooting selfTankShooting = null;

    /// <summary>ターゲットの前フレームでの座標</summary>
    private Vector3 _oldPosition = Vector3.zero;

    /// <summary>ターゲットの前フレームでの発射準備状態</summary>
    //private bool oldLaunchPrepare = false;

    /// <summary>
    /// OnEnable()より前に1度だけ呼ばれる
    /// </summary>
    private void Awake()
    {

#if GUI_OUTPUT
        gui_instanceNum = BasePlayer.gui_instanceTotalNum++;
#endif
    }

    // Start is called before the first frame update
    /// <summary>
    /// 開始時に１度呼ばれる
    /// </summary>
    void Start()
    {
        obstacleLayerMask = LayerMask.GetMask("FieldObject");
        _selfAnimator = GetComponent<Animator>();
    }


    // Update is called once per frame
    /// <summary>
    /// ゲームループで（1秒間に何回も）呼ばれる
    /// </summary>
    private void Update()
    {
        // ターゲットの情報収集

        // AIの更新
        UpdateAI();

        // 出力の調整
        Quaternion q = Quaternion.AngleAxis(selfFrontAngle, Vector3.up);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, this.transform.rotation * q, 1);

        // 次フレームのために情報を残す
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
    virtual protected void GUIOutputSelfInfo()
    {
#if GUI_OUTPUT
        GUILayout.Label("SelfPosition: " + transform.position);
        GUILayout.Label("SelfYRotation: " + transform.rotation.eulerAngles.y);
#endif  // GUI_OUTPUT
    }
}
