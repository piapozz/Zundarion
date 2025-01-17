using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CollisionAction")]
public class CollisionAction : ScriptableObject
{
    public enum CollisionTag
    {
        PLAYER,      // プレイヤー
        ENEMY,       // エネミー

        MAX
    }

    public enum CollisionLayer
    {
        ATTACK,      // 攻撃
        ATTACK_OMEN,        // 攻撃予兆

        MAX
    }

    public string[] collisionTags;       // 当たり判定のタグ
    public string[] collisionLayers;     // 当たり判定のレイヤー
}
