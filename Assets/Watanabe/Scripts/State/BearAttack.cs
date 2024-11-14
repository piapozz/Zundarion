using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAttack : IEnemyState
{
    public void Action(EnemyBase.EnemyStatus enemyStatus)
    {
        enemyStatus.m_animator.SetTrigger("Attack");
    }
}
