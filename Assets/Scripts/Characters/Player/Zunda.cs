using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Zunda : BasePlayer
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>�A�j���[�^�[�f�[�^</summary>
    [SerializeField]
    private AnimationData playerAnimation = null;

    /// <summary>�h����ɂ�鏉����</summary>
    protected override void Init()
    {
        selfAnimationData = playerAnimation;
        selfComboCountMax = 3;
    }


}
