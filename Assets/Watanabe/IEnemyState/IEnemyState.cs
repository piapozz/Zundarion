/*
 * @file IEnemyState.cs
 * @brief 敵のステート
 * @author sein
 * @date 2025/2/14
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void Enter(BaseEnemy enemy);
    void Execute(BaseEnemy enemy);
    void Exit(BaseEnemy enemy);
}
