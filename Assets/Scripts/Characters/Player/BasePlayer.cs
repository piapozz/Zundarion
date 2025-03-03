using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading;

using static CommonModule;
using static GameConst;

public abstract class BasePlayer : BaseCharacter
{
    // public //////////////////////////////////////////////////////////////////

    /// <summary>�v���C���[�̏����ݒ�</summary>
    public virtual void Setup()
    {

    }

    /// <summary>���g�̑O���A���O��</summary>
    public float selfFrontAngleZ { get; set; }

    /// <summary>���g�̃Q�[���I�u�W�F�N�g</summary>
    public GameObject selfGameObject { get; private set; }

    // protected //////////////////////////////////////////////////////////////////

    /// <summary>�h����ɂ�鏉����</summary>
    protected abstract void Init();

    // private //////////////////////////////////////////////////////////////////

    /// <summary>���͂��ꂽ�ړ�����</summary>
    private Vector2 _inputMoveDir = Vector2.zero;

    /// <summary>�ړ��Ɋ|����{��</summary>
    private float _currentMultiplier = 1.0f;

    /// <summary>�d�������ǂ���</summary>
    private bool _isStiff = false;

    /// <summary>�v���C���[�̐�s���͏��</summary>
    [SerializeField]
    private PreInput _selfPreInput = null;

    /// <summary>�p���B�̃X�g�b�N</summary>
    private int _parryStock = 0;

    /// <summary>����̃X�g�b�N</summary>
    private int _avoidStock = 0;

    /// <summary>�p���B�̃N�[���_�E���^�X�N</summary>
    private UniTask _parryCoolDownTask;

    /// <summary>�p���B�̃N�[���_�E���L�����Z���g�[�N��</summary>
    private CancellationTokenSource _parryCTS = null;

    /// <summary>����̃N�[���_�E���^�X�N</summary>
    private UniTask _avoidCoolDownTask;

    /// ����̃N�[���_�E���L�����Z���g�[�N��
    private CancellationTokenSource _avoidCTS = null;

    private const float _RUN_SPEED_RATE = 1.5f;
    private const float _ATTACK_SENS_RANGE = 10.0f;
    private const int _PARRY_COOL_DOWN_STOCK = 2;
    private const float _PARRY_COOL_DOWN_SECOND = 2.0f;
    private const int _AVOID_COOL_DOWN_STOCK = 2;
    private const float _AVOID_COOL_DOWN_SECOND = 2.0f;

    void Awake()
    {
        _selfPreInput.Initialize();

        _parryStock = _PARRY_COOL_DOWN_STOCK;
        _avoidStock = _AVOID_COOL_DOWN_STOCK;

        Init();
    }

    private void Update()
    {
        // �ړ�����
        MoveExecute();

        // ��s���͏���
        PreInputExecute();
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
    /// �ړ����s����
    /// </summary>
    private void MoveExecute()
    {
        selfAnimator.SetBool("Move", false);

        // �ړ��ł��Ȃ��Ȃ珈���𔲂���
        if (_isStiff) return;

        // �ړ����������͂���Ă���Ȃ�
        if (_inputMoveDir.x != 0 || _inputMoveDir.y != 0)
        {
            selfAnimator.SetBool("Move", true);
        }
        else
        {
            _currentMultiplier = 1.0f;
            return;
        }

        // ��]���ړ�
        Rotate(AdjustMoveDir());
        Move(speed * _currentMultiplier);
    }

    /// <summary>
    /// ��s���͂ɉ����ď���
    /// </summary>
    private void PreInputExecute()
    {
        InputType input = _selfPreInput.preInputType;
        // �d���������͂���ĂȂ��Ȃ珈�����Ȃ�
        if (_isStiff || input == InputType.None) return;

        // �e���͂̏���
        switch (input)
        {
            case InputType.Run:
                AvoidExecute();
                break;
            case InputType.Attack:
                AttackExecute();
                break;
            case InputType.Parry:
                ParryExecute();
                break;
        }
        _selfPreInput.ClearRecord();
    }

    /// <summary>
    /// ����A�N�V���������s
    /// </summary>
    /// <returns></returns>
    private void AvoidExecute()
    {
        if (_isStiff) return;

        // �N�[���_�E�����Ȃ珈���𔲂���
        if (CheckAvoidCoolDown()) return;

        // �W���X�g����ɂȂ邩����
        List<BaseCharacter> parryList = CollisionManager.instance.parryList;
        if (parryList.Count <= 0)
        {
            selfAnimator.SetTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.AVOID]);
            _currentMultiplier = _RUN_SPEED_RATE;
        }
        else
        {
            // �A�j���[�V�������Z�b�g
            selfAnimator.SetTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.JUST_AVOID]);
        }
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

    /// <summary>
    /// �U������
    /// </summary>
    public void AttackExecute()
    {
        // �A�j���[�V�����ݒ�
        selfAnimator.SetTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.ATTACK]);
        // �G�̕���������
        BaseCharacter character = CharacterManager.instance.GetNearCharacter(this, _ATTACK_SENS_RANGE);
        if (character == null) return;
        TurnAround(character.gameObject.transform);
    }

    /// <summary>
    /// �p���B����
    /// </summary>
    public void ParryExecute()
    {
        // �p���B�N�[���_�E�����Ȃ珈���𔲂���
        if (CheckParryCoolDown()) return;
        // �p���B�ɂȂ邩����
        List<BaseCharacter> parryList = CollisionManager.instance.parryList;
        if (parryList.Count <= 0)
        {
            // �A�j���[�V�������Z�b�g
            selfAnimator.SetTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.PARRY_MISS]);
        }
        else
        {
            // �A�j���[�V�������Z�b�g
            selfAnimator.SetTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.PARRY]);
            // �v���C���[��G�̕����Ɍ�����
            TurnAround(parryList[0].transform);
            // �ʏ�J���������Z�b�g
            CameraManager.instance.SetFreeCam(transform.eulerAngles.y, 0.5f);
            // �p���B����̃A�j���[�V�������Ђ�݂ɂ���
            parryList[0].SetImpact();
        }
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

    /// <summary>
    /// ���S�d����ݒ�
    /// </summary>
    /// <param name="second"></param>
    public void SetStiffEvent(int frame)
    {
        _isStiff = true;
        UniTask task = WaitAction(frame, () => _isStiff = false);
    }

    /// <summary>
    /// �p���B���̃X���[��ݒ�
    /// </summary>
    public void SetSlowEvent()
    {
        SlowManager.instance.SetSlow(PARRY_SLOW_SPEED, PARRY_SLOW_TIME);
    }

    public void StartTimeEvent()
    {
        SlowManager.instance.StartTime();
    }

    public override bool IsPlayer() { return true; }

    public override void TakeDamage(float damageSize, float strength)
    {
        base.TakeDamage(damageSize, strength);

        if (health <= 0)
        {
            selfAnimator.SetTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.DIE]);
            CharacterManager.instance.RemoveCharacterList(ID);
        }
        else
        {
            // ��s���͂��L�����Z��
            _selfPreInput.ClearRecord();
            // �Ђ��
            SetImpact();
        }
    }

    public override void SetImpact()
    {
        selfAnimator.SetTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.IMPACT]);
    }

    public override void DeadEvent()
    {
        base.DeadEvent();
        UniTask task = FadeManager.instance.TransScene("GameResult", SCENE_FADE_TIME);
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
