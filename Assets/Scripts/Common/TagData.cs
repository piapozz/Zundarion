using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TagData")]
public class TagData : ScriptableObject
{
    public enum CollisionTag
    {
        PLAYER,      // プレイヤー
        ENEMY,       // エネミー

        MAX
    }

    public string[] collisionTags;       // 当たり判定のタグ
}
