using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CollisionAction")]
public class CollisionAction : ScriptableObject
{
    public enum CollisionTag
    {
        ATTACK_NOMAL,       // 通常攻撃
        ATTACK_PASSABLE,    // パリィ可能
        ATTACK_DANGEROUS,   // パリィ不可
        AVOIDANCE,          // 回避
    }

    public enum CollisionLayer
    {
        PLAYER_ATTACK,      // プレイヤー攻撃
        PLAYER_SURVIVE,     // プレイヤー受け手
        ENEMY_ATTACK,       // エネミー攻撃
        ENEMY_SURVIVE,      // エネミー受け手
    }

    public string[] collisionTags;       // 当たり判定のタグ
    public string[] collisionLayers;     // 当たり判定のレイヤー
}
