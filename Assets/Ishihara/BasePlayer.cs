using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;
using UnityEngine.InputSystem;

public abstract class BasePlayer : MonoBehaviour
{
    // public //////////////////////////////////////////////////////////////////

    /// <summary>�v���C���[�̏����ݒ�</summary>
    public virtual void Setup()
    {
       
    }

    /// <summary>�A�j���[�^�[�R���|�[�l���g</summary>
    public Animator selfAnimator = null;

    /// <summary>���g�̌��݂̗̑�</summary>
    //public float selfCurrentHealth { get => selfTankHealth?.CurrentHealth ?? 0.0f; }

    /// <summary>�U���R���{�̍ő吔</summary>
    public float selfComboCount { get; protected set; }

    /// <summary>���݂̈ړ��X�e�[�g</summary>
    public PlayerAnimation.MoveAnimation selfMoveState  { get; set; }

    /// <summary>�v���C���[�̈ړ����x</summary>
    public float selfMoveSpeed { get; protected set; }

    /// <summary>�A�j���[�V�����̍Đ����x</summary>
    public float selfAnimationSpeed { get; protected set; }

    /// <summary>�A�j���[�V�����̃p�����[�^�[���</summary>
    public PlayerAnimation selfAnimationData { get; protected set; }

    /// <summary>�A�j���[�V�����̃p�����[�^�[���</summary>
    public CollisionAction selfCollisionData { get; protected set; }

    /// <summary>���g�̑O���A���O��</summary>
    public float selfFrontAngleZ { get; set; }

    // protected //////////////////////////////////////////////////////////////////

    /// <summary>�h����ɂ�鏉����</summary>
    protected abstract void Init();

    /// <summary>���t���[���Ă΂��AI�ɂ�鑀��</summary>
    // protected abstract void UpdateAI();

    /// <summary>��Q���̃��C���[�}�X�N</summary>
    protected LayerMask obstacleLayerMask { get; private set; }

    /// <summary>�v���C���[�̓��͕���</summary>
    protected Vector2 inputMove = Vector3.zero;

    /// <summary>��Ԃ̈ړ��R���|�[�l���g</summary>
    protected bool inputRun = false;

    // private //////////////////////////////////////////////////////////////////

    /// <summary>�v���C���[�̈ړ��R���|�[�l���g</summary>
    private PlayerMove _selfMove ;

    /// <summary>�v���C���[�̓���</summary>
    private PlayerInput _playerInput ;

    /// <summary>�v���C���[�̈ړ����͏��</summary>
    private InputAction _inputAction;

    /// <summary>���������󂯎��Ώۂ�Action</summary>
    private InputActionReference hold;

    /// <summary>��Ԃ̖C�e���˃R���|�[�l���g</summary>
    //private TankShooting selfTankShooting = null;

    /// <summary>�^�[�Q�b�g�̑O�t���[���ł̍��W</summary>
    private Vector3 _oldPosition = Vector3.zero;

    // Start is called before the first frame update
    /// <summary>
    /// �J�n���ɂP�x�Ă΂��
    /// </summary>
    void Awake()
    {
        obstacleLayerMask = LayerMask.GetMask("FieldObject");
        _playerInput = GetComponent<PlayerInput>();
        _inputAction = _playerInput.actions["Move"];
        _selfMove = GetComponent<PlayerMove>();
        selfMoveState = PlayerAnimation.MoveAnimation.IDLE;

        Init();

        if (hold == null) return;

        // InputActionReference��hold�Ƀn���h����o�^����
        hold.action.performed += OnRun;

        // ���͂��󂯎�邽�߂ɗL����
        hold.action.Enable();


    }


    // Update is called once per frame
    /// <summary>
    /// �Q�[�����[�v�Łi1�b�Ԃɉ�����j�Ă΂��
    /// </summary>
    private void Update()
    {
        // �^�[�Q�b�g�̏����W


        if(inputRun) selfMoveState = PlayerAnimation.MoveAnimation.AVOIDANCE;
        else selfMoveState = PlayerAnimation.MoveAnimation.WALK;

        if(inputMove == Vector2.zero) selfMoveState = PlayerAnimation.MoveAnimation.IDLE;

        // AI�̍X�V
        // UpdateAI();

        _selfMove.Move(inputMove, selfMoveState);

        if (inputMove == Vector2.zero) return;

        // �o�͂̒���
        Quaternion q = Quaternion.AngleAxis(selfFrontAngleZ - 90, Vector3.down);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation,  q, 5);


        // ���t���[���̂��߂ɏ����c��

    }

    // �A�N�V�����}�b�v��Move�ɓo�^����Ă���L�[�������ꂽ�Ƃ��ɓ��͒l���擾
    public void OnMove(InputAction.CallbackContext context)
    {
        inputMove = context.ReadValue<Vector2>();
    }

    // Actions��Run�Ɋ��蓖�Ă��Ă�����͂��������Ȃ���s
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() != 0) inputRun = true;
        else inputRun = false;
    }


#if GUI_OUTPUT

    /// <summary>GUI�o�͗p �C���X�^���X�J�E���^</summary>
    static private int gui_instanceTotalNum = 0;

    /// <summary>GUI�o�͗p �C���X�^���X�ԍ�</summary>
    private int gui_instanceNum;

    /// <summary>�C���X�y�N�^�pGUI�\���^��\���t���O</summary>
    [SerializeField]
    private bool enableGUIOutput = true;

    /// <summary>
    /// ���t���[���Ă΂��GUI�o�͗p���\�b�h
    /// <para>
    /// ��ʂɃf�o�b�O�p�̏����o��
    /// �i�d���̂Ŏ��@�ɂ͏��Ȃ��悤�ɂ���j
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

    /// <summary>GUI�Ɏ��g�̏����o��</summary>
    protected void GUIOutputSelfInfo()
    {
#if GUI_OUTPUT
        GUILayout.Label("SelfPosition: " + transform.position);
        GUILayout.Label("SelfYRotation: " + transform.rotation.eulerAngles.y);
        GUILayout.Label("input.x: " + _inputMove.z "input.y" + _inputMove.y);
#endif  // GUI_OUTPUT
    }
}
