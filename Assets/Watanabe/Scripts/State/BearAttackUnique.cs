using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAttackUnique : IEnemyState
{

    CreateCollision.AttackData attackData;

    public void Action(EnemyBase.EnemyStatus enemyStatus)
    {
        enemyStatus.m_animator.SetTrigger("AttackUnique");

        CreateCollision.instance.CreateCollisionSphere(enemyStatus.m_gameObject, attackData);
    }
}
