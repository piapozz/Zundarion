using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAttack", menuName = "CharacterAttack")]
public class CharacterAttackData : ScriptableObject
{
    public bool isParry;        // パリィかどうか
    public float distance;      // 距離
    public float radius;        // 半径
    public float damage;        // ダメージ
    public float generateTime;  // 生成している時間
}
