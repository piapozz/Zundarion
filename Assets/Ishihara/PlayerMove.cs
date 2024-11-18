using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEditor.Animations;

public class PlayerMove : MonoBehaviour
{
    // public //////////////////////////////////////////////////////////////////

    public enum MoveState
    {
        IDLE,
        WALK,
        AVOIDANCE,
        RUN,

        MAX
    }

    // private //////////////////////////////////////////////////////////////////

    /// <summary>�v���C���[�̃A�j���[�^�[</summary>
    private Animator _animator = null;

    /// <summary>�v���C���[�̐e�R���{�[�l���g</summary>
    private BasePlayer _palyer = null;

    /// <summary>�A�j���[�V�����p�����[�^�[�̏��</summary>
    private PlayerAnimation _animationPram;

    /// <summary>����̍Đ�����</summary>
    private float _avoidanceTime = 0f;


    // Start is called before the first frame update
    /// <summary>
    /// �J�n���ɂP�x�Ă΂��
    /// </summary>
    void Start()
    {
        _animator = GetComponent<Animator>();       // �A�j���[�^�[�擾
        _palyer = GetComponent<BasePlayer>();       // �v���C���[�擾
        _animationPram = _palyer.selfAnimationData; // �A�j���[�V�����f�[�^�擾

        AnimatorController controller = _animator.runtimeAnimatorController as AnimatorController;
        if (controller == null) return;

        foreach (var layer in controller.layers)
        {
            foreach (var state in layer.stateMachine.states)
            {
                if (state.state.name == _animationPram.movePram[(int)MoveState.AVOIDANCE])
                {
                    Motion motion = state.state.motion;
                    if (motion is AnimationClip clip)
                    {
                        _avoidanceTime = clip.length;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    /// <summary>
    /// �Q�[�����[�v�Łi1�b�Ԃɉ�����j�Ă΂��
    /// </summary>
    void FixedUpdate()
    {
        // 0 ���C���[�̍Đ�����Ă���A�j���[�V���������Ăяo��
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // ����ɂȂ����瑖��ɑJ�ڂ�����
        if (stateInfo.IsName(_animationPram.movePram[(int)MoveState.AVOIDANCE]) &&
            _palyer.selfMoveState !=�@MoveState.RUN)
            _palyer.selfMoveState = MoveState.RUN;
           
    }

    /// <summary>
    /// ���͂ɉ����Ċp�x�̕ύX�ƃA�j���[�V�����̍Đ�������
    /// </summary>
    public void Move(Vector3 moveVec, MoveState moveState)
    {
        // �ړ��ł���A�j���[�V�����󋵂Ȃ�
        if (!CheckAssailable()) return;

        // ���݂̈ړ��X�e�[�g���ς���Ă����Ȃ�t���O��؂�ւ���
        if (CheckChangeMoveState(moveState)) ChangeMoveState(moveState);

        // ��������͂̕����Ɍ���
        float temp;

        temp = Mathf.Atan2(moveVec.z, moveVec.x);

        _palyer.selfFrontAngleZ = temp * Mathf.Rad2Deg;
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
        result = stateInfo.IsName(_animationPram.movePram[(int)MoveState.IDLE]) || 
            stateInfo.IsName(_animationPram.movePram[(int)MoveState.WALK]) || 
            stateInfo.IsName(_animationPram.movePram[(int)MoveState.AVOIDANCE]) || 
            stateInfo.IsName(_animationPram.movePram[(int)MoveState.RUN]);

        return result;
    }

    /// <summary>
    /// ���݂̈ړ��X�e�[�g���ς���Ă��邩���ׂ�
    /// true�Ȃ�ς���Ă���
    /// </summary>
    bool CheckChangeMoveState(MoveState state)
    {
        // ����
        bool result = false;

        // 0 ���C���[�̍Đ�����Ă���A�j���[�V���������Ăяo��
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // ���݂̃X�e�[�g�ƃA�j���[�V����������Ă�����
        result = stateInfo.IsName(_animationPram.movePram[(int)state]);

        return result;
    }

    /// <summary>
    /// �A�j���[�V����Index�ɐ؂�ւ��Ƃ���ɔ�������������
    /// </summary>
    void ChangeMoveState(MoveState state)
    {
        // MoveIndex��ύX
        switch (state)
        {
            case MoveState.IDLE:
            case MoveState.WALK:
            case MoveState.RUN:
            {
                _animator.SetInteger("Move", (int)state);
                break;
            }


            case MoveState.AVOIDANCE:
            {
                _animator.SetInteger("Move", (int)state);

                    // ��𓖂��蔻��𐶐�

                    // �p�����[�^�[������
                    CreateCollision.AttackData data = new CreateCollision.AttackData().zero();

                    data.position = _palyer.transform.position;
                    data.radius = 2;
                    data.layer = "Player";
                    data.tagname = "Avoidance";
                    data.time = _avoidanceTime;

                    // ����
                    CreateCollision.instance.CreateCollisionSphere(_palyer.gameObject, data);

                break;
            }

        }
    }
}
