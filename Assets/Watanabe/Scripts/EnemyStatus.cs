using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Editor�ォ��쐬���邽�߂̋L�q
[CreateAssetMenu(menuName = "EnemyStatusObject")]

public class EnemyStatus : ScriptableObject
{
    public string aiName;

    public int health;
    public int speed;
    public int strength;
}
