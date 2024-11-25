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

    /// <summary>アニメーターデータ</summary>
    [SerializeField]
    private PlayerAnimation playerAnimation = null;

    /// <summary>プレイヤーの移動データ</summary>
    [SerializeField]
    private CollisionAction collisionAction = null;

    /// <summary>プレイヤーの当たり判定発生データ</summary>
    [SerializeField]
    private OccurrenceFrame occurrenceFrame = null;

    /// <summary>派生先による初期化</summary>
    protected override void Init()
    {
        selfAnimationData = playerAnimation;
        selfCollisionData = collisionAction;
        selfOccurrenceFrame = occurrenceFrame;
        selfComboCountMax = 3;
    }


}
