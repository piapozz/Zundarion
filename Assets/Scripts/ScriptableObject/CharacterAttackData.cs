using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAttack", menuName = "ScriptableObjects/CharacterAttack")]
public class CharacterAttackData : ScriptableObject
{
    public bool isParry;        // パリィかどうか
    public float distance;      // 距離
    public float scale;         // 大きさ
    public float damage;        // ダメージ
    public float generateTime;  // 生成している時間
}
