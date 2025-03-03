/*
 * @file EnemyBear.cs
 * @brief Bear‚ðŠÇ—‚·‚éƒNƒ‰ƒX
 * @author sein
 * @date 2025/1/17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBear : BaseEnemy
{
    // “{‚èƒQ[ƒW
    [SerializeField] private int fury = -1;
    private readonly int _ENEMY_FURY_INITIAL = 0;
    private readonly int _ENEMY_FURY_INCREASE = 6;

    private void Start()
    {
        ChangeState(new BearIdleState());
        fury = _ENEMY_FURY_INITIAL;
    }

    private void Update()
    {
        // Debug.Log(enemyState);
        position = transform.position;
        Debug.Log(enemyState);
    }

    public override void TakeDamage(float damageSize, float strength)
    {
        base.TakeDamage(damageSize, strength);
        fury += _ENEMY_FURY_INCREASE;

    }

    public int GetFury() { return fury; }

    // “{‚èƒQ[ƒW‚ð•Ï“®‚³‚¹‚é
    public void ChangeFury(int changeValue)
    {
        if (changeValue == 0) return;

        if (changeValue >= 1) fury += changeValue;

        else if (changeValue <= -1)
        {
            int index = fury + changeValue;
            if (index >= 0) fury = index;
            else if (index <= 0) fury = 0;
        }
    }

}
