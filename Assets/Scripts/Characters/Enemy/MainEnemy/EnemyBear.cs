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
        position = transform.position;
    }

    protected override void DamageReaction()
    {
        fury += _ENEMY_FURY_INCREASE;
    }

    public int GetFury() { return fury; }

    // �{��Q�[�W��ϓ�������
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
