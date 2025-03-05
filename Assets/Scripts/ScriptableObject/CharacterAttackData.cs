using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAttack", menuName = "ScriptableObjects/CharacterAttack")]
public class CharacterAttackData : ScriptableObject
{
    public bool isParry;        // パリィかどうか
    public bool isAvoid;        // 回避かどうか
    public float distance;      // 距離
    public float scale;         // 大きさ
    public float damage;        // ダメージ
    public int deleteFrame;     // 生成している時間
}
