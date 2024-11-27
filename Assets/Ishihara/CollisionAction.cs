using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CollisionAction")]
public class CollisionAction : ScriptableObject
{
    public enum CollisionTag
    {
        ATTACK_NORMAL,       // 攻撃
        ATTACK_PASSABLE,    // パリィ可能予兆
        ATTACK_DANGEROUS,   // パリィ不可予兆
        AVOIDANCE,          // 回避

        MAX
    }

    public enum CollisionLayer
    {
        PLAYER_ATTACK,      // プレイヤー攻撃
        PLAYER_SURVIVE,     // プレイヤー受け手
        ENEMY_ATTACK,       // エネミー攻撃
        ENEMY_SURVIVE,      // エネミー受け手

        MAX
    }

    public string[] collisionTags;       // 当たり判定のタグ
    public string[] collisionLayers;     // 当たり判定のレイヤー
}
