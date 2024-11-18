using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zunda : BasePlayer
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>�A�j���[�^�[�R���|�[�l���g</summary>
    [SerializeField]
    private PlayerAnimation playerAnimation = null;

    /// <summary>�v���C���[�̈ړ��R���|�[�l���g</summary>
    [SerializeField]
    private CollisionAction collisionAction = null;

    /// <summary>�h����ɂ�鏉����</summary>
    protected override void Init()
    {
        selfAnimationData = playerAnimation;
        selfCollisionData = collisionAction;
    }
}
