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
    public MoveAnimation selfMoveState  { get; set; } = MoveAnimation.IDLE;

    /// <summary>�A�j���[�V�����̍Đ����x</summary>
    public float selfAnimationSpeed { get; protected set; }

    /// <summary>�A�j���[�V�����̃p�����[�^�[���</summary>
    public PlayerAnimation selfAnimationData { get; protected set; }

    /// <summary>���g�̑O���A���O��</summary>
    public float selfFrontAngleZ { get; set; }

    /// <summary>���g�̃Q�[���I�u�W�F�N�g</summary>
    public GameObject selfGameObject { get; private set; }

    // protected //////////////////////////////////////////////////////////////////

    /// <summary>�h����ɂ�鏉����</summary>
    protected abstract void Init();

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
    private InputActionReference _hold;

    /// <summary>�^�[�Q�b�g�̑O�t���[���ł̍��W</summary>
    private Vector3 _oldPosition = Vector3.zero;

    /// <summary>����\���ǂ���</summary>
    private bool _operable = true;

    /// <summary>���͂��ꂽ�ړ�����</summary>
    private Vector2 _inputMoveDir = Vector2.zero;

    /// <summary>�ړ��Ɋ|����{��</summary>
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

        // InputActionReference��hold�Ƀn���h����o�^����
        _hold.action.performed += OnRun;

        // ���͂��󂯎�邽�߂ɗL����
        _hold.action.Enable();
    }

    private void Update()
    {

        Debug.Log(selfMoveState);
        MoveExecute();

        /*
        // �^�[�Q�b�g�̏����W
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

        // �o�͂̒���
        Quaternion q = Quaternion.AngleAxis(selfFrontAngleZ - 90, Vector3.down);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation,  q, 1.5f);
        */
    }

    /// <summary>
    /// �A�N�V�����}�b�v��Move�ɓo�^����Ă���L�[�������ꂽ�Ƃ��ɓ��͒l���擾
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        _inputMoveDir = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Actions��Run�Ɋ��蓖�Ă��Ă�����͂��������Ȃ���s
    /// </summary>
    /// <param name="context"></param>
    public void OnRun(InputAction.CallbackContext context)
    {

        UniTask task = RunExecute();
    }

    /// <summary>
    /// Actions��Attak�Ɋ��蓖�Ă��Ă�����͂��������Ȃ���s
    /// </summary>
    /// <param name="context"></param>
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

    public override bool IsPlayer() { return true; }

    public override void TakeDamage(float damageSize)
    {
        base.TakeDamage(damageSize);
        if (health <= 0)
            selfAnimator.SetTrigger(selfAnimationData.anyStatePram[(int)AnyStateAnimation.DIE]);
    }

    /// <summary>
    /// �ړ����s����
    /// </summary>
    private void MoveExecute()
    {
        if (!_operable) return;
        if (_inputMoveDir.x == 0 && _inputMoveDir.y == 0)
        {
            // �A�j���[�V�����ݒ�
            selfMoveState = MoveAnimation.IDLE;
            _currentMultiplier = 0.0f;
        }

        else
        {
            selfMoveState = MoveAnimation.WALK;
            _currentMultiplier = 1.0f;
        }

        // ��]���ړ�
        Rotate(AdjustMoveDir());

        // �A�j���[�V������ύX
        selfAnimator.SetInteger("Move", (int)selfMoveState);

        Move(speed * _currentMultiplier);
    }

    /// <summary>
    /// ����A�N�V���������s
    /// </summary>
    /// <returns></returns>
    private async UniTask RunExecute()
    {
        if (!_operable) return;

        SetAvoidState();
        // ������I���܂őҋ@
        while (CheckAnimation(selfAnimationData.anyStatePram[(int)AnyStateAnimation.AVOID]))
        {
            await UniTask.DelayFrame(1);
        }
        SetRunState();
    }

    /// <summary>
    /// ���͏�񂩂�ړ������𒲐�
    /// </summary>
    /// <returns></returns>
    private Vector3 AdjustMoveDir()
    {
        // �J�����̕����Ɋ�Â��ē��̓x�N�g�����C��
        Vector3 cameraForward = CameraManager.instance.selfCamera.transform.forward;
        Vector3 cameraRight = CameraManager.instance.selfCamera.transform.right;
        // �J���������Ɋ�Â����ړ��x�N�g�����v�Z
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
