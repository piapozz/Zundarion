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
        AVOIDANCE,
        RUN,

        MAX
    }

    public enum AttackAnimation
    {
        ATTACK,

        MAX
    }

    public enum ChangeAnimation
    {
        PARRY,
        TRANS_FRONTLINE,
        TRANS_DUPLICATE,

        MAX
    }

    public enum InterruqtAnimation
    {
        TRANS_ENHANCED,
        IMPACT,
        VICTORY,
        DIE,

        MAX
    }

    public string[] movePram;       // 移動アニメーション
    public string[] attackPram;     // アタックアニメーション
    public string[] changePram;     // キャラ変更アニメーション
    public string[] interruptPram;  // 割り込みアニメーション
}
