using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAttack", menuName = "CharacterAttack")]
public class CharacterAttackData : ScriptableObject
{
    public bool isParry;        // �p���B���ǂ���
    public float distance;      // ����
    public float radius;        // ���a
    public float damage;        // �_���[�W
    public float generateTime;  // �������Ă��鎞��
}
