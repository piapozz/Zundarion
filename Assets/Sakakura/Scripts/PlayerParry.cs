using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static CommonModule;

public class PlayerParry : MonoBehaviour
{
    /// <summary>�v���C���[�̐e�R���{�[�l���g</summary>
    private BasePlayer _player = null;

    /// <summary>�v���C���[�̃A�j���[�^�[</summary>
    private Animator _animator = null;

    /// <summary>�v���C���[�̓����蔻��`�F�b�N</summary>
    private CheckCollision _checkCollision = null;

    private bool _isCoolDown = false;

    public readonly float PARRY_COOL_DOWN = 0.0f;

    private void Start()
    {
        _player = GetComponent<BasePlayer>();           // �v���C���[�擾
        _animator = _player.selfAnimator;               // �A�j���[�^�[�擾
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
        _animator.SetTrigger("Parry");
        // �p���B����̃A�j���[�V�������Ђ�݂ɂ���
        parryList[0].selfAnimator.SetTrigger("Impact");
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
