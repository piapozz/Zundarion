using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEditor.Animations;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerMove : MonoBehaviour
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>�v���C���[�̃A�j���[�^�[</summary>
    private Animator _animator = null;

    /// <summary>�v���C���[�̐e�R���{�[�l���g</summary>
    private BasePlayer _player = null;

    /// <summary>�A�j���[�V�����p�����[�^�[�̏��</summary>
    private PlayerAnimation _animationPram = null;

    /// <summary>�A�j���[�V�����p�����[�^�[�̏��</summary>
    private CollisionAction _collisionPram;

    /// <summary>����̍Đ�����</summary>
    private float _avoidanceTime = 0f;


    // Start is called before the first frame update
    /// <summary>
    /// �J�n���ɂP�x�Ă΂��
    /// </summary>
    void Start()
    {
        _player = GetComponent<BasePlayer>();       // �v���C���[�擾
        _animator = _player.selfAnimator;       // �A�j���[�^�[�擾
        _animationPram = _player.selfAnimationData; // �A�j���[�V�����f�[�^�擾
        _collisionPram = _player.selfCollisionData; // �R���W�����f�[�^�擾

        AnimatorController controller = _animator.runtimeAnimatorController as AnimatorController;
        if (controller == null) return;

        foreach (var layer in controller.layers)
        {
            foreach (var state in layer.stateMachine.states)
            {
                if (state.state.name == _animationPram.movePram[(int)PlayerAnimation.MoveAnimation.AVOIDANCE])
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
    void Update()
    {
        // 0 ���C���[�̍Đ�����Ă���A�j���[�V���������Ăяo��
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // ����ɂȂ����瑖��ɑJ�ڂ�����
        if (stateInfo.IsName(_animationPram.movePram[(int)PlayerAnimation.MoveAnimation.AVOIDANCE]) &&
            _player.selfMoveState != PlayerAnimation.MoveAnimation.RUN)
            _player.selfMoveState = PlayerAnimation.MoveAnimation.RUN;
           
    }

    /// <summary>
    /// ���͂ɉ����Ċp�x�̕ύX�ƃA�j���[�V�����̍Đ�������
    /// </summary>
    public void Move(Vector2 moveVec, PlayerAnimation.MoveAnimation moveState)
    {
        // �ړ��ł���A�j���[�V�����󋵂Ȃ�
        if (!CheckAssailable()) return;

        // ���݂̈ړ��X�e�[�g���ς���Ă����Ȃ�t���O��؂�ւ���
        if (CheckChangeMoveState(moveState)) ChangeMoveState(moveState);

        // ��������͂̕����Ɍ���
        float temp;

        temp = Mathf.Atan2(moveVec.y, moveVec.x);

        _player.selfFrontAngleZ = (temp * Mathf.Rad2Deg);
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

                    // ��𓖂��蔻��𐶐�

                    // �p�����[�^�[������
                    CreateCollision.AttackData data = new CreateCollision.AttackData().zero();

                    data.position = _player.transform.position;
                    data.radius = 2;
                    data.layer = _collisionPram.collisionLayers[(int)CollisionAction.CollisionLayer.PLAYER_SURVIVE];
                    data.tagname = _collisionPram.collisionTags[(int)CollisionAction.CollisionTag.AVOIDANCE];
                    data.time = _avoidanceTime;

                    // ����
                    CreateCollision.instance.CreateCollisionSphere(_player.gameObject, data);

                break;
            }

        }
    }
}
