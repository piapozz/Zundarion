using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;
using UnityEngine.InputSystem;
using static PlayerAnimation;

public abstract class BasePlayer : BaseCharacter
{
    // public //////////////////////////////////////////////////////////////////

    /// <summary>�v���C���[�̏����ݒ�</summary>
    public virtual void Setup()
    {
       
    }

    /// <summary>���g�̌��݂̗̑�</summary>
    public float selfCurrentHealth { get; protected set; }

    /// <summary>�U���R���{�̍ő吔</summary>
    public int selfComboCountMax { get; protected set; }

    /// <summary>�U���R���{��</summary>
    public int selfComboCount { get; protected set; }

    /// <summary>���݂̈ړ��X�e�[�g</summary>
    public PlayerAnimation.MoveAnimation selfMoveState  { get; set; }

    /// <summary>�v���C���[�̈ړ����x</summary>
    public float selfMoveSpeed { get; protected set; }

    /// <summary>�A�j���[�V�����̍Đ����x</summary>
    public float selfAnimationSpeed { get; protected set; }

    /// <summary>�A�j���[�V�����̃p�����[�^�[���</summary>
    public PlayerAnimation selfAnimationData { get; protected set; }

    /// <summary>�����蔻����</summary>
    public TagData selfCollisionData { get; protected set; }

    /// <summary>�����蔻�蔭�����</summary>
    public OccurrenceFrame selfOccurrenceFrame { get; protected set; }

    /// <summary>���g�̑O���A���O��</summary>
    public float selfFrontAngleZ { get; set; }

    /// <summary>���g�̃Q�[���I�u�W�F�N�g</summary>
    public GameObject selfGameObject { get; private set; }

    /// <summary>�v���C���[�̓����蔻��`�F�b�N</summary>
    public CheckCollision selfCheckCollision { get; protected set; }

    // protected //////////////////////////////////////////////////////////////////

    /// <summary>�h����ɂ�鏉����</summary>
    protected abstract void Init();

    /// <summary>���t���[���Ă΂��AI�ɂ�鑀��</summary>
    // protected abstract void UpdateAI();

    /// <summary>��Q���̃��C���[�}�X�N</summary>
    protected LayerMask obstacleLayerMask { get; private set; }

    /// <summary>�v���C���[�̓��͕���</summary>
    protected Vector2 inputMove = Vector3.zero;

    /// <summary>�������</summary>
    protected bool inputRun = false;

    /// <summary>�U������</summary>
    protected bool inputAttack = false;

    /// <summary>�p���B����</summary>
    protected bool inputParry = false;

    // private //////////////////////////////////////////////////////////////////

    /// <summary>�v���C���[�̈ړ��R���|�[�l���g</summary>
    private PlayerMove _selfMove ;

    /// <summary>�v���C���[�̍U���R���|�[�l���g</summary>
    private PlayerAttack _selfAttack;

    /// <summary>�v���C���[�̃p���B�R���|�[�l���g</summary>
    private PlayerParry _selfParry;

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
        _selfAttack = GetComponent<PlayerAttack>();
        _selfParry = GetComponent<PlayerParry>();
        selfMoveState = PlayerAnimation.MoveAnimation.IDLE;
        selfGameObject = this.gameObject;
        selfCheckCollision = GetComponent<CheckCollision>();

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
        this.transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if(inputRun) 
            selfMoveState = PlayerAnimation.MoveAnimation.AVOIDANCE;
        else if(selfMoveState != PlayerAnimation.MoveAnimation.RUN) 
            selfMoveState = PlayerAnimation.MoveAnimation.WALK;

        if(inputMove == Vector2.zero && selfMoveState != PlayerAnimation.MoveAnimation.AVOIDANCE) 
            selfMoveState = PlayerAnimation.MoveAnimation.IDLE;

        // AI�̍X�V
        // UpdateAI();

        _selfMove.Move(inputMove, selfMoveState);

        if(inputAttack)
            _selfAttack.Attack();

        if (inputMove == Vector2.zero) 
            return;

        // �o�͂̒���
        Quaternion q = Quaternion.AngleAxis(selfFrontAngleZ - 90, Vector3.down);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation,  q, 1.5f);

        // ���t���[���̂��߂ɏ����c��

    }

    /// <summary>
    /// �v���C���[�Ƀ_���[�W��^����
    /// </summary>
    /*
    public void TakeDamage(float damage)
    {
        // ��𒆂Ȃ�_���[�W��H���Ȃ�
        // 0 ���C���[�̍Đ�����Ă���A�j���[�V���������Ăяo��
        AnimatorStateInfo stateInfo = selfAnimator.GetCurrentAnimatorStateInfo(0);

        // ����I�ɃR���{�񐔂�����������
        if (stateInfo.IsName("Avoidance") || stateInfo.IsName("Parry")) return;

        selfAnimator.SetTrigger(selfAnimationData.interruptPram[(int)InterruqtAnimation.IMPACT]);
        selfCurrentHealth -= damage;
    }*/

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

    // Actions��Attak�Ɋ��蓖�Ă��Ă�����͂��������Ȃ���s
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() != 0) inputAttack = true;
        else inputAttack = false;
    }

    /// <summary>
    /// Actions��Parry�Ɋ��蓖�Ă��Ă�����͂��������Ȃ���s
    /// </summary>
    /// <param name="context"></param>
    public void OnParry(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        _selfParry.Parry();
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
