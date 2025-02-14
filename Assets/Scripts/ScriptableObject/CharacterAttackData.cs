using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAttack", menuName = "ScriptableObjects/CharacterAttack")]
public class CharacterAttackData : ScriptableObject
{
    public bool isParry;        // �p���B���ǂ���
    public float distance;      // ����
    public float scale;         // �傫��
    public float damage;        // �_���[�W
    public float generateTime;  // �������Ă��鎞��
}
