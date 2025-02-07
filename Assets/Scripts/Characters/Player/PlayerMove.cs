using System.Collections;
using UnityEngine;
using UnityEditor.Animations;

public class PlayerMove : BaseAction
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>�v���C���[�̃A�j���[�^�[</summary>
    private Animator _animator = null;

    /// <summary>�v���C���[�̐e�R���{�[�l���g</summary>
    private BasePlayer _player = null;

    /// <summary>�A�j���[�V�����p�����[�^�[�̏��</summary>
    private PlayerAnimation _animationPram = null;

    /// <summary>�A�j���[�V�����p�����[�^�[�̏��</summary>
    private TagData _collisionPram;

    /// <summary>����̍Đ�����</summary>
    private float _avoidanceTime = 0f;

    private System.Action<Vector3, float> _Move = null;

    public void SetCallback(System.Action<Vector3, float> action)
    {
        _Move = action;
    }

    public void Initialize()
    {

    }

    public void Execute()
    {
        /*
        // 0 ���C���[�̍Đ�����Ă���A�j���[�V���������Ăяo��
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // ����ɂȂ����瑖��ɑJ�ڂ�����
        if (stateInfo.IsName(_animationPram.movePram[(int)PlayerAnimation.MoveAnimation.AVOIDANCE]) &&
            _player.selfMoveState != PlayerAnimation.MoveAnimation.RUN)
            _player.selfMoveState = PlayerAnimation.MoveAnimation.RUN;
        */
    }

    /*
    /// <summary>
    /// ���͂ɉ����Ċp�x�̕ύX�ƃA�j���[�V�����̍Đ�������
    /// </summary>
    public void Move(Vector2 moveVec, PlayerAnimation.MoveAnimation moveState)
    {
        // �ړ��ł���A�j���[�V�����󋵂Ȃ�
        if (!CheckAssailable()) return;

        // ���݂̈ړ��X�e�[�g���ς���Ă����Ȃ�t���O��؂�ւ���
        if (CheckChangeMoveState(moveState)) ChangeMoveState(moveState);

        // �J�����̕����Ɋ�Â��ē��̓x�N�g�����C��
        Vector3 cameraForward = CameraManager.instance.selfCamera.transform.forward;
        Vector3 cameraRight = CameraManager.instance.selfCamera.transform.right;

        // �J���������Ɋ�Â����ړ��x�N�g�����v�Z
        Vector3 adjustedMove = (cameraRight * moveVec.x + cameraForward * moveVec.y).normalized;

        // �������v�Z���čX�V
        float angle = Mathf.Atan2(adjustedMove.z, adjustedMove.x) * Mathf.Rad2Deg;

        _player.selfFrontAngleZ = angle;
    }

    /// <summary>
    /// ���݂̃A�j���[�V�������ړ��\�ȏ�ԂȂ̂����ׂ�
    /// </summary>
    bool CheckAssailable()
    {
        // ����
        bool result = false;

        // 0 ���C���[�̍Đ�����Ă���A�j���[�V���������Ăяo��
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // ���݂̃A�j���[�V������Idle,Move,Run,Avoid�Ȃ�true
        result = stateInfo.IsName(_animationPram.movePram[(int)PlayerAnimation.MoveAnimation.IDLE]) ||
            stateInfo.IsName(_animationPram.movePram[(int)PlayerAnimation.MoveAnimation.WALK]) ||
            stateInfo.IsName(_animationPram.movePram[(int)PlayerAnimation.MoveAnimation.AVOIDANCE]) ||
            stateInfo.IsName(_animationPram.movePram[(int)PlayerAnimation.MoveAnimation.RUN]);

        return result;
    }

    /// <summary>
    /// ���݂̈ړ��X�e�[�g���ς���Ă��邩���ׂ�
    /// true�Ȃ�ς���Ă���
    /// </summary>
    bool CheckChangeMoveState(PlayerAnimation.MoveAnimation state)
    {
        // ����
        bool result = true;

        // 0 ���C���[�̍Đ�����Ă���A�j���[�V���������Ăяo��
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // ���݂̃X�e�[�g�ƃA�j���[�V����������Ă�����
        result = _animator.GetInteger("Move") != (int)state;

        return result;
    }

    /// <summary>
    /// �A�j���[�V����Index�ɐ؂�ւ��Ƃ���ɔ�������������
    /// </summary>
    void ChangeMoveState(PlayerAnimation.MoveAnimation state)
    {
        // MoveIndex��ύX
        switch (state)
        {
            case PlayerAnimation.MoveAnimation.IDLE:
            case PlayerAnimation.MoveAnimation.WALK:
            case PlayerAnimation.MoveAnimation.RUN:
                {
                    _animator.SetInteger("Move", (int)state);
                    break;
                }


            case PlayerAnimation.MoveAnimation.AVOIDANCE:
                {
                    _animator.SetInteger("Move", (int)state);
                    break;
                }

        }
    }
    */
}
