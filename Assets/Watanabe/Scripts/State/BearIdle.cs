using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearIdle : IEnemyState
{
    public void Action(EnemyBase.EnemyStatus enemyStatus)
    {
        enemyStatus.m_animator.SetBool("Found", false);
    }
}
