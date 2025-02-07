using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.StandaloneInputModule;
using UnityEngine.InputSystem;

public class Zunda : BasePlayer
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>�A�j���[�^�[�f�[�^</summary>
    [SerializeField]
    private PlayerAnimation playerAnimation = null;

    /// <summary>�v���C���[�̓����蔻�蔭���f�[�^</summary>
    [SerializeField]
    private OccurrenceFrame occurrenceFrame = null;

    /// <summary>�v���C���[�̍ő�HP</summary>
    [SerializeField]
    private int MaxHP = 100;

    /// <summary>�h����ɂ�鏉����</summary>
    protected override void Init()
    {
        selfAnimationData = playerAnimation;
        selfComboCountMax = 3;
        selfCurrentHealth = MaxHP;
    }


}
