using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>�v���C���[�̃A�j���[�^�[</summary>
    private Animator _animator = null;

    /// <summary>�v���C���[�̐e�R���{�[�l���g</summary>
    private BasePlayer _player = null;

    /// <summary>�����蔻�萶���R���|�[�l���g</summary>
    private CreateCollision _createCollision = null;

    /// <summary>�A�j���[�V�����p�����[�^�[�̏��</summary>
    private PlayerAnimation _animationPram = null;

    /// <summary>�A�j���[�V�����p�����[�^�[�̏��</summary>
    private CollisionAction _collisionPram;

    /// <summary>���݂̃R���{��</summary>
    private int _comboCount = 0;

    /// <summary>�A�j���[�V�����̖��O</summary>
    private string[] _comboName =
    {
        "Attack_1",
        "Attack_2",
        "Attack_3"
    };

    // Start is called before the first frame update
    /// <summary>
    /// �J�n���ɂP�x�Ă΂��
    /// </summary>
    void Start()
    {
        _player = GetComponent<BasePlayer>();       // �v���C���[�擾
        _animator = _player.selfAnimator;           // �A�j���[�^�[�擾
        _animationPram = _player.selfAnimationData; // �A�j���[�V�����f�[�^�擾
        _collisionPram = _player.selfCollisionData; // �R���W�����f�[�^�擾
    }

    // Update is called once per frame
    /// <summary>
    /// �Q�[�����[�v�Łi1�b�Ԃɉ�����j�Ă΂��
    /// </summary>
    void Update()
    {
        // 0 ���C���[�̍Đ�����Ă���A�j���[�V���������Ăяo��
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // ����I�ɃR���{�񐔂�����������
        if(stateInfo.IsName("Idle")) _comboCount = 0;
    }

    /// <summary>
    /// �A�j���[�V�������U���ɍX�V���āA�����蔻��𐶐�����
    /// </summary>
    public void Attack()
    {
        // �R���{�̔h�����܂�����Ȃ�
        if(_comboCount >= _player.selfComboCountMax) return;

        // �U���ł���A�j���[�V�����󋵂Ȃ�
        if(!CheckAssailable()) return;

        // �J�E���^�[�𑝂₵�āB�g���K�[���Z�b�g����
        _comboCount++;
        _animator.SetTrigger(_animationPram.attackPram[(int)PlayerAnimation.AttackAnimation.ATTACK]);
    }

    /// <summary>
    /// ���݂̃A�j���[�V�������U���\�ȏ�ԂȂ̂����ׂ�
    /// </summary>
    bool CheckAssailable()
    {
        bool result;

        // 0 ���C���[�̍Đ�����Ă���A�j���[�V���������Ăяo��
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // ���݂̃X�e�[�g�ƃA�j���[�V����������Ă�����
        result = !stateInfo.IsName(_comboName[_player.selfComboCount]);

        return result;
    }

}
