//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerMove : MonoBehaviour
//{
//    // public //////////////////////////////////////////////////////////////////

//    public enum MoveState
//    {
//        WALK,
//        RUN,

//        MAX
//    }

//    // private //////////////////////////////////////////////////////////////////

//    /// <summary>�v���C���[�̃A�j���[�^�[</summary>
//    private Animator _animator = null;

//    /// <summary>�v���C���[�̐e�R���{�[�l���g</summary>
//    private BasePlayer _palyer = null;

//    /// <summary>�����蔻�萶���R���|�[�l���g</summary>
//    //private TankMovement selfTankMovement = null;

//    /// <summary>���݂̈ړ��X�e�[�g</summary>
//    private MoveState _moveState;


//    // Start is called before the first frame update
//    /// <summary>
//    /// �J�n���ɂP�x�Ă΂��
//    /// </summary>
//    void Start()
//    {
//        _animator = GetComponent<Animator>();   // �A�j���[�^�[�擾
//        _palyer = GetComponent<BasePlayer>();   // �v���C���[�擾

//        // �g���K�[�p�����[�^�[�������ݒ�
//        for (int i = 0; i < _palyer.SelfComboCount; i++)
//        {
//            _triggers.Add("Attack" + (i + 1).ToString());
//        }
//    }

//    // Update is called once per frame
//    /// <summary>
//    /// �Q�[�����[�v�Łi1�b�Ԃɉ�����j�Ă΂��
//    /// </summary>
//    void Update()
//    {
//        // ����I�ɃR���{�񐔂�����������
//        _comboCount = 0;
//    }

//    // Update is called once per frame
//    /// <summary>
//    /// ���͂ɉ����Ċp�x�̕ύX�ƃA�j���[�V�����̍Đ�������
//    /// </summary>
//    void Move(Vector3 moveVec)
//    {
//        // �R���{�̔h�����܂�����Ȃ�
//        if (_comboCount >= _palyer.SelfComboCount) return;

//        // �U���ł���A�j���[�V�����󋵂Ȃ�
//        if (!CheckAssailable()) return;

//        // �J�E���^�[�𑝂₵�āB�g���K�[���Z�b�g����
//        _comboCount++;
//        _animator.SetTrigger(_triggers[_comboCount]);

//        // �����蔻��𐶐�����

//    }

//    // Update is called once per frame
//    /// <summary>
//    /// ���݂̃A�j���[�V�������U���\�ȏ�ԂȂ̂����ׂ�
//    /// </summary>
//    bool CheckAssailable()
//    {
//        return true;
//    }

//}
