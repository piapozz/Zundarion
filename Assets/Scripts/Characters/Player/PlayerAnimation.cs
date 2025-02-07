using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/PlayerAnimation")]
public class PlayerAnimation : ScriptableObject
{
    public enum MoveAnimation
    {
        IDLE = 0,
        WALK,
        RUN,

        MAX
    }

    public enum AttackAnimation
    {
        ATTACK,

        MAX
    }

    public enum AnyStateAnimation
    {
        IMPACT,
        PARRY,
        AVOID,
        VICTORY,
        DIE,

        MAX
    }

    public string[] movePram;       // �ړ��A�j���[�V����
    public string[] attackPram;     // �A�^�b�N�A�j���[�V����
    public string[] anyStatePram;   // �ǂ�����ł��J�ڂ���A�j���[�V����
}
