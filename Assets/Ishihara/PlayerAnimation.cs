using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/PlayerAnimation")]
public class PlayerAnimation : ScriptableObject
{
    enum Animation
    {
        IDLE = 0,
        WALK,
        AVOIDANCE,
        RUN,
        ATTACK,
        PARRY,
        TRANS_FRONTLINE,
        TRANS_DUPLICATE,
        TRANS_ENHANCED,
        IMPACT,
        VICTORY,
        DIE,

        MAX
    }

    public string[] movePram;       // �ړ��A�j���[�V����
    public string[] attackPram;     // �A�^�b�N�A�j���[�V����
    public string[] changePram;     // �L�����ύX�A�j���[�V����
    public string[] interruptPram;  // ���荞�݃A�j���[�V����
}
