using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    /// <summary>�v���C���[�̐e�R���{�[�l���g</summary>
    private BasePlayer _player = null;

    /// <summary>�v���C���[�̃A�j���[�^�[</summary>
    private Animator _animator = null;

    /// <summary>�v���C���[�̓����蔻��`�F�b�N</summary>
    private CheckCollision _checkCollision = null;

    private void Start()
    {
        _player = GetComponent<BasePlayer>();           // �v���C���[�擾
        _animator = _player.selfAnimator;               // �A�j���[�^�[�擾
        _checkCollision = _player.selfCheckCollision;
    }

    public void Parry()
    {
        // �p���B�ɂȂ邩����
        if (_checkCollision.parryList.Count == 0) return;
        Debug.Log(_checkCollision.parryList.Count);
        // �A�j���[�V�������Z�b�g
        _animator.SetTrigger("Parry");
        // �p���B����̃A�j���[�V�������Ђ�݂ɂ���
        _checkCollision.parryList[0].selfAnimator.SetTrigger("Impact");
        // �v���C���[��G�̕����Ɍ�����
        _player.TurnAround(_checkCollision.parryList[0].transform);
        // �ʏ�J���������Z�b�g
        CameraManager.instance.SetFreeCam(transform.eulerAngles.y, 0.5f);
    }
}
