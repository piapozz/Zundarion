/*
 * @file EnemyBear.cs
 * @brief Bear���Ǘ�����N���X
 * @author sein
 * @date 2025/1/17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBear : BaseEnemy
{
    // �{��Q�[�W
    public float fury = -1;
    private readonly float _ENEMY_INCREASE_FURY = 10.0f;

    private void Start()
    {
        ChangeState(new BearWanderingState());
    }

    private void Update()
    {
        // Debug.Log(enemyState);
        position = transform.position;
    }

    public override void TakeDamage(float damageSize, float strength)
    {
        base.TakeDamage(damageSize, strength);
        fury += _ENEMY_INCREASE_FURY;

    }
}
