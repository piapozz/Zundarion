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
    public MoveAnimation selfMoveState { get; set; } = MoveAnimation.IDLE;

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
    private PlayerMove _selfMove;

    /// <summary>�v���C���[�̍U���R���|�[�l���g</summary>
    private PlayerAttack _selfAttack;

    /// <summary>�v���C���[�̃p���B�R���|�[�l���g</summary>
    private PlayerParry _selfParry;

    /// <summary>�v���C���[�̓���</summary>
    private PlayerInput _playerInput;

    /// <summary>�v���C���[�̈ړ����͏��</summary>
    private InputAction _inputAction;

    /// <summary>���������󂯎��Ώۂ�Action</summary>
    private InputActionReference _hold;

    /// <summary>�^�[�Q�b�g�̑O�t���[���ł̍��W</summary>
    private Vector3 _oldPosition = Vector3.zero;

    /// <summary>���͂��ꂽ�ړ�����</summary>
    private Vector2 _inputMoveDir = Vector2.zero;

    /// <summary>�ړ��Ɋ|����{��</summary>
    private float _currentMultiplier = 1.0f;

    /// <summary>�ړ��d�������ǂ���</summary>
    private bool _isMoveStiff = false;

    /// <summary>�d�������ǂ���</summary>
    private bool _isAllStiff = false;

    /// <summary>�v���C���[�̐�s���͏��</summary>
    private PreInput _selfPreInput = null;

    /// <summary>�p���B�̃X�g�b�N</summary>
    private int _parryStock = 0;

    /// <summary>����̃X�g�b�N</summary>
    private int _avoidStock = 0;

    /// <summary>�p���B�̃N�[���_�E���^�X�N</summary>
    private UniTask _parryCoolDownTask;

    private CancellationTokenSource _parryCTS = null;

    /// <summary>����̃N�[���_�E���^�X�N</summary>
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
        if (!context.performed || _isAllStiff) return;
        UniTask task = RunExecute();
    }

    /// <summary>
    /// Actions��Attak�Ɋ��蓖�Ă��Ă�����͂��������Ȃ���s
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || _isAllStiff) return;
        Attack();
    }

    /// <summary>
    /// Actions��Parry�Ɋ��蓖�Ă��Ă�����͂��������Ȃ���s
    /// </summary>
    /// <param name="context"></param>
    public void OnParry(InputAction.CallbackContext context)
    {
        if (!context.performed || _isAllStiff) return;
        Parry();
    }

    /// <summary>
    /// �ړ����s����
    /// </summary>
    private void MoveExecute()
    {
        // �ړ��ł��Ȃ��Ȃ珈���𔲂���
        if (_isAllStiff || _isMoveStiff) return;
        // �ړ����������͂���Ă��Ȃ��Ȃ珈���𔲂���
        if (_inputMoveDir.x == 0 && _inputMoveDir.y == 0)
        {
            selfAnimator.SetBool("Move", false);
            return;
        }
        selfAnimator.SetBool("Move", true);

        if (selfMoveState == MoveAnimation.IDLE)
            SetWalkState();

        // ��]���ړ�
        Rotate(AdjustMoveDir());
        Move(speed * _currentMultiplier);
    }

    /// <summary>
    /// ����A�N�V���������s
    /// </summary>
    /// <returns></returns>
    private async UniTask RunExecute()
    {
        if (_isAllStiff) return;

        // �N�[���_�E�����Ȃ珈���𔲂���
        if (CheckAvoidCoolDown()) return;

        SetAvoidState();
        while (!CheckAnimation(selfAnimationData.anyStatePram[(int)AnyStateAnimation.AVOID]))
        {
            await UniTask.DelayFrame(1);
        }
        // ������I���܂őҋ@
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
        // �N�[���_�E�����Ȃ�L�����Z��
        if (!_avoidCoolDownTask.Status.IsCompleted())
            _avoidCTS.Cancel();

        _avoidCoolDownTask = WaitAction(_AVOID_COOL_DOWN_SECOND, () => _avoidStock = _AVOID_COOL_DOWN_STOCK, _avoidCTS.Token);

        return false;
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
    /// �U������
    /// </summary>
    public void Attack()
    {
        // �A�j���[�V�����ݒ�
        selfAnimator.SetTrigger(selfAnimationData.attackPram[(int)AttackAnimation.ATTACK]);
        // �G�̕���������
        BaseCharacter character = CharacterManager.instance.GetNearCharacter(this, _ATTACK_SENS_RANGE);
        if (character == null) return;
        TurnAround(character.gameObject.transform);
    }

    /// <summary>
    /// �p���B����
    /// </summary>
    public void Parry()
    {
        // �p���B�ɂȂ邩����
        List<BaseCharacter> parryList = CollisionManager.instance.parryList;
        if (parryList.Count == 0) return;
        // �p���B�N�[���_�E�����Ȃ珈���𔲂���
        if (CheckParryCoolDown()) return;
        // �A�j���[�V�������Z�b�g
        selfAnimator.SetTrigger(selfAnimationData.anyStatePram[(int)AnyStateAnimation.PARRY]);
        // �p���B����̃A�j���[�V�������Ђ�݂ɂ���
        parryList[0].selfAnimator.SetTrigger(selfAnimationData.anyStatePram[(int)AnyStateAnimation.IMPACT]);
        // �v���C���[��G�̕����Ɍ�����
        TurnAround(parryList[0].transform);
        // �ʏ�J���������Z�b�g
        CameraManager.instance.SetFreeCam(transform.eulerAngles.y, 0.5f);
    }

    /// <summary>
    /// �p���B�N�[���_�E������
    /// </summary>
    /// <returns></returns>
    private bool CheckParryCoolDown()
    {
        if (_parryStock <= 0) return true;

        _parryStock--;
        _parryCTS = new CancellationTokenSource();
        // �N�[���_�E�����Ȃ�L�����Z��
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
