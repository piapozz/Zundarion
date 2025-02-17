using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Zunda : BasePlayer
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>アニメーターデータ</summary>
    [SerializeField]
    private AnimationData playerAnimation = null;

    /// <summary>派生先による初期化</summary>
    protected override void Init()
    {
        selfAnimationData = playerAnimation;
        selfComboCountMax = 3;
    }


}
