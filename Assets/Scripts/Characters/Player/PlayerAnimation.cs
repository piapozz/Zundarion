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

    public string[] movePram;       // 移動アニメーション
    public string[] attackPram;     // アタックアニメーション
    public string[] anyStatePram;   // どこからでも遷移するアニメーション
}
