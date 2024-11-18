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

    /// <summary>アニメーターコンポーネント</summary>
    [SerializeField]
    private PlayerAnimation playerAnimation = null;

    /// <summary>プレイヤーの移動コンポーネント</summary>
    [SerializeField]
    private CollisionAction collisionAction = null;

    /// <summary>派生先による初期化</summary>
    protected override void Init()
    {
        selfAnimationData = playerAnimation;
        selfCollisionData = collisionAction;
    }


}
