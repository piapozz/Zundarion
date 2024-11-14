using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>�v���C���[�̃A�j���[�^�[</summary>
    private Animator _animator = null;

    /// <summary>�v���C���[�̐e�R���{�[�l���g</summary>
    private BasePlayer _palyer = null;

    /// <summary>�����蔻�萶���R���|�[�l���g</summary>
    //private TankMovement selfTankMovement = null;

    /// <summary>���݂̃R���{��</summary>
    private int _comboCount = 0;

    /// <summary>�v���C���[�̃A�j���[�^�[�g���K�[</summary>
    private List<string> _triggers;


    // Start is called before the first frame update
    /// <summary>
    /// �J�n���ɂP�x�Ă΂��
    /// </summary>
    void Start()
    {
        _animator = GetComponent<Animator>();   // �A�j���[�^�[�擾
        _palyer = GetComponent<BasePlayer>();   // �v���C���[�擾

        // �g���K�[�p�����[�^�[�������ݒ�
        for(int i = 0;i< _palyer.selfComboCount; i++)
        {
            _triggers.Add("Attack" + (i + 1).ToString());
        }
    }

    // Update is called once per frame
    /// <summary>
    /// �Q�[�����[�v�Łi1�b�Ԃɉ�����j�Ă΂��
    /// </summary>
    void Update()
    {
        // ����I�ɃR���{�񐔂�����������
        _comboCount = 0;
    }

    /// <summary>
    /// �A�j���[�V�������U���ɍX�V���āA�����蔻��𐶐�����
    /// </summary>
    void Attack()
    {
        // �R���{�̔h�����܂�����Ȃ�
        if(_comboCount >= _palyer.selfComboCount) return;

        // �U���ł���A�j���[�V�����󋵂Ȃ�
        if(!CheckAssailable()) return;

        // �J�E���^�[�𑝂₵�āB�g���K�[���Z�b�g����
        _comboCount++;
        _animator.SetTrigger(_triggers[_comboCount]);

        // �����蔻��𐶐�����

    }

    /// <summary>
    /// ���݂̃A�j���[�V�������U���\�ȏ�ԂȂ̂����ׂ�
    /// </summary>
    bool CheckAssailable()
    {
        return true;
    }

}
