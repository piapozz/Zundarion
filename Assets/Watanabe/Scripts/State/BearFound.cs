using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearFound : IEnemyState
{
    public void Action(EnemyBase.EnemyStatus enemyStatus)
    {
        enemyStatus.m_animator.SetBool("Found", true);
    }
}
