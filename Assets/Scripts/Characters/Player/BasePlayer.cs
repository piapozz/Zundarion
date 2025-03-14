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

    /// <summary>�U���ȊO�̍d��</summary>
    private bool _isNonAttackStiff = false;

    /// <summary>�v���C���[�̐�s���͏��</summary>
    [SerializeField]
    private PreInput _selfPreInput = null;

    /// <summary>�d���^�X�N</summary>
    private int _stiffTaskCount = 0;

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

    private List<CollisionData> _parryList = null;

    private List<CollisionData> _avoidList = null;

    private const float _RUN_SPEED_RATE = 1.5f;         // ���鑬�x�{��
    private const float _ATTACK_SENS_RANGE = 5.0f;     // �U�����m�͈�
    private const int _PARRY_COOL_DOWN_STOCK = 2;       // �p���B�N�[���_�E���X�g�b�N
    private const float _PARRY_COOL_DOWN_SECOND = 2.0f; // �p���B�N�[���_�E���b��
    private const int _AVOID_COOL_DOWN_STOCK = 2;       // ����N�[���_�E���X�g�b�N
    private const float _AVOID_COOL_DOWN_SECOND = 2.0f; // ����N�[���_�E���b��
    private const int _ATTACK_CAMERA_FRAME = 20;        // �U�����̃J�����J�ڃt���[��
    private const float _IMPACT_HEALTH_RATIO = 0.05f;   // �ő勯�ݒl�̗͔̑䗦(0�`1)

    void Awake()
    {
        _selfPreInput.Initialize();

        _parryStock = _PARRY_COOL_DOWN_STOCK;
        _avoidStock = _AVOID_COOL_DOWN_STOCK;

        _parryList = new List<CollisionData>(5);
        _avoidList = new List<CollisionData>(5);

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
        if (_isStiff || _isNonAttackStiff) return;

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
        if (_isStiff || _isNonAttackStiff) return;

        // �N�[���_�E�����Ȃ珈���𔲂���
        if (CheckAvoidCoolDown()) return;

        // �W���X�g����ɂȂ邩����
        if (_avoidList.Count <= 0)
        {
            SetAnimationTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.AVOID]);
            _currentMultiplier = _RUN_SPEED_RATE;
        }
        else
        {
            if (_avoidList[0] == null) return;

            TurnNearEnemy();
            // �A�j���[�V�������Z�b�g
            SetAnimationTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.JUST_AVOID]);
            // �X���[
            SlowManager.instance.SetSlow(AVOID_SLOW_SPEED, AVOID_SLOW_TIME);
        }
    }

    /// <summary>
    /// ����̃N�[���_�E���`�F�b�N
    /// </summary>
    /// <returns></returns>
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
    private void AttackExecute()
    {
        TurnNearEnemy();
        // �A�j���[�V�����ݒ�
        SetAnimationTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.ATTACK]);
        // �J�����𒲐�
        UniTask task = CameraManager.instance.SetFreeCam(transform.eulerAngles.y, 0.5f, _ATTACK_CAMERA_FRAME);
    }

    /// <summary>
    /// �߂��̓G�ɐ��񂷂�
    /// </summary>
    private void TurnNearEnemy()
    {
        // �߂��̓G���擾���p�x����
        BaseCharacter character = CharacterManager.instance.GetNearCharacter(this, _ATTACK_SENS_RANGE);
        if (character == null) return;

        targetEnemy = character;
        TurnAround(targetEnemy.transform);
    }

    /// <summary>
    /// �p���B����
    /// </summary>
    private void ParryExecute()
    {
        if (_isStiff || _isNonAttackStiff) return;

        // �p���B�N�[���_�E�����Ȃ珈���𔲂���
        if (CheckParryCoolDown()) return;

        // �p���B�ɂȂ邩����
        if (_parryList.Count <= 0)
        {
            // �A�j���[�V�������Z�b�g
            SetAnimationTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.PARRY_MISS]);
        }
        else
        {
            if (_parryList[0] == null) return;
            // �A�j���[�V�������Z�b�g
            SetAnimationTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.PARRY]);
            // �v���C���[��G�̕����Ɍ�����
            targetEnemy = CharacterManager.instance.GetCharacter(_parryList[0].characterID);
            TurnAround(targetEnemy.transform);
            // �ʏ�J���������Z�b�g
            UniTask task = CameraManager.instance.SetFreeCam(transform.eulerAngles.y, 0.5f);
            // �p���B����̃A�j���[�V�������Ђ�݂ɂ���
            targetEnemy.SetImpact();
        }
    }

    public void AddParryList(CollisionData targetCollision)
    {
        if (_parryList.Exists(collision => collision == targetCollision)) return;
        _parryList.Add(targetCollision);
    }

    public void RemoveParryList(CollisionData targetCollision)
    {
        if (_parryList.Exists(collision => collision == targetCollision))
            _parryList.Remove(targetCollision);
    }

    public void AddAvoidList(CollisionData targetCollision)
    {
        if (_avoidList.Exists(chara => chara == targetCollision)) return;
        _avoidList.Add(targetCollision);
    }

    public void RemoveAvoidList(CollisionData targetCollision)
    {
        if (_avoidList.Exists(collision => collision == targetCollision))
            _avoidList.Remove(targetCollision);
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

        UniTask task = WaitAction(frame, ClearStiff);
        _stiffTaskCount++;
    }

    private void ClearStiff()
    {
        // �����Ă��鏈����1�Ȃ�d������
        if (_stiffTaskCount <= 1) _isStiff = false;

        _stiffTaskCount--;
    }

    /// <summary>
    /// �ړ��d����ݒ�
    /// </summary>
    /// <param name="frame"></param>
    public void SetMoveStiffEvent(int frame)
    {
        _isNonAttackStiff = true;
        UniTask task = WaitAction(frame, () => _isNonAttackStiff = false);
    }

    /// <summary>
    /// �d������
    /// </summary>
    public void ClearStiffEvent()
    {
        _isStiff = false;
        _isNonAttackStiff = false;
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

    protected override void OnDead()
    {
        base.OnDead();
        SetAnimationTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.DIE]);
        isDead = true;
        GameManager.SetGameOver();
    }

    protected override void TakeImpact(float impact)
    {
        base.TakeImpact(impact);

        if (impactValue >= healthMax * _IMPACT_HEALTH_RATIO)
        {
            impactValue = 0;
            SetImpact();
        }
    }

    public override void SetImpact()
    {
        // ��s���͂��L�����Z��
        _selfPreInput.ClearRecord();
        SetAnimationTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.IMPACT]);
    }

    public override void DeadEvent()
    {
        base.DeadEvent();
        UniTask task = FadeManager.instance.TransScene("GameResult", SCENE_FADE_TIME);
    }

    private void SetAnimationTrigger(string triggerName)
    {
        if (isDead) return;
        selfAnimator.SetTrigger(triggerName);
    }
}
