using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static CommonModule;
using static PlayerAnimation;

public class PlayerParry : MonoBehaviour
{
    /// <summary>�v���C���[�̐e�R���{�[�l���g</summary>
    private BasePlayer _player = null;

    /// <summary>�v���C���[�̃A�j���[�^�[</summary>
    private Animator _animator = null;

    /// <summary>�A�j���[�V�����p�����[�^�[�̏��</summary>
    private PlayerAnimation _animationPram = null;

    /// <summary>�v���C���[�̓����蔻��`�F�b�N</summary>
    private CheckCollision _checkCollision = null;

    private bool _isCoolDown = false;

    public readonly float PARRY_COOL_DOWN = 0.0f;

    private void Start()
    {
        _player = CharacterManager.instance.player;     // �v���C���[�擾
        _animator = _player.selfAnimator;               // �A�j���[�^�[�擾
        _animationPram = _player.selfAnimationData;
        _checkCollision = _player.selfCheckCollision;
    }

    public void Parry()
    {
        List<BaseCharacter> parryList = CollisionManager.instance.parryList;
        // �p���B�ɂȂ邩����
        if (parryList.Count == 0) return;
        if (_isCoolDown == true) return;
        // �N�[���_�E���J�n
        _isCoolDown = true;
        UniTask task = WaitAction(PARRY_COOL_DOWN, UpCoolDown);
        // �A�j���[�V�������Z�b�g
        _animator.SetTrigger(_animationPram.changePram[(int)ChangeAnimation.PARRY]);
        // �p���B����̃A�j���[�V�������Ђ�݂ɂ���
        parryList[0].selfAnimator.SetTrigger(_animationPram.interruptPram[(int)InterruqtAnimation.IMPACT]);
        // �v���C���[��G�̕����Ɍ�����
        _player.TurnAround(parryList[0].transform);
        // �ʏ�J���������Z�b�g
        CameraManager.instance.SetFreeCam(transform.eulerAngles.y, 0.5f);
    }

    public void UpCoolDown()
    {
        _isCoolDown = false;
    }
}
