/*
 * @file EnemyBear.cs
 * @brief Bearを管理するクラス
 * @author sein
 * @date 2025/1/17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBear : BaseEnemy
{
    // 怒りゲージ
    public float fury = -1;

    private void Start()
    {
        ChangeState(new BearWanderingState());
    }

    private void Update()
    {
        Debug.Log(enemyState);
        position = transform.position;
    }
}
