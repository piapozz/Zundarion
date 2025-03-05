using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackEyeData", menuName = "ScriptableObjects/EnemyAttackEyeData")]
public class EnemyAttackEyeData : ScriptableObject
{
    // �p���B�ł��邩�ǂ���
    public bool isParry;
    // ����ł��邩�ǂ���
    public bool isAvoid;
    // �����܂ł̎���
    public float deleteTime;
}
