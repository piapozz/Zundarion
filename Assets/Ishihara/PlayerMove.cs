using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // public //////////////////////////////////////////////////////////////////

    public enum MoveState
    {
        WALK,
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


    // Start is called before the first frame update
    /// <summary>
    /// �J�n���ɂP�x�Ă΂��
    /// </summary>
    void Start()
    {
        _animator = GetComponent<Animator>();       // �A�j���[�^�[�擾
        _palyer = GetComponent<BasePlayer>();       // �v���C���[�擾
        _animationPram = _palyer.selfAnimationData; // �A�j���[�V�����f�[�^�擾
    }

    // Update is called once per frame
    /// <summary>
    /// �Q�[�����[�v�Łi1�b�Ԃɉ�����j�Ă΂��
    /// </summary>
    void Update()
    {

    }

    /// <summary>
    /// ���͂ɉ����Ċp�x�̕ύX�ƃA�j���[�V�����̍Đ�������
    /// </summary>
    void Move(Vector3 moveVec, MoveState moveState)
    {
        // �R���{�̔h�����܂�����Ȃ�
        if (!(moveVec == Vector3.zero)) return;

        // �ړ��ł���A�j���[�V�����󋵂Ȃ�
        if (!CheckAssailable()) return;

        // �t���O��؂�ւ���
        // _animator.SetBool(_triggers[_comboCount]);

        // ��������͂̕����Ɍ���
        _palyer.selfFrontAngle = this.transform.forward.x;
    }

    /// <summary>
    /// ���݂̃A�j���[�V�������ړ��\�ȏ�ԂȂ̂����ׂ�
    /// </summary>
    bool CheckAssailable()
    {
        return true;
    }

}
