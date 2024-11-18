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
    private CreateCollision _createCollision = null;

    /// <summary>�A�j���[�V�����p�����[�^�[�̏��</summary>
    private PlayerAnimation _animationPram = null;

    /// <summary>�A�j���[�V�����p�����[�^�[�̏��</summary>
    private CollisionAction _collisionPram;

    /// <summary>���݂̃R���{��</summary>
    private int _comboCount = 0;

    // Start is called before the first frame update
    /// <summary>
    /// �J�n���ɂP�x�Ă΂��
    /// </summary>
    void Start()
    {
        _animator = GetComponent<Animator>();   // �A�j���[�^�[�擾
        _palyer = GetComponent<BasePlayer>();   // �v���C���[�擾
        _animationPram = _palyer.selfAnimationData; // �A�j���[�V�����f�[�^�擾
        _collisionPram = _palyer.selfCollisionData; // �R���W�����f�[�^�擾
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
        _animator.SetTrigger(_animationPram.attackPram[(int)PlayerAnimation.AttackAnimation.ATTACK]);

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
