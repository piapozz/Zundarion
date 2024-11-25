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

    /// <summary>�v���C���[�̈ړ��f�[�^</summary>
    [SerializeField]
    private CollisionAction collisionAction = null;

    /// <summary>�v���C���[�̓����蔻�蔭���f�[�^</summary>
    [SerializeField]
    private OccurrenceFrame occurrenceFrame = null;

    /// <summary>�h����ɂ�鏉����</summary>
    protected override void Init()
    {
        selfAnimationData = playerAnimation;
        selfCollisionData = collisionAction;
        selfOccurrenceFrame = occurrenceFrame;
        selfComboCountMax = 3;
    }


}
