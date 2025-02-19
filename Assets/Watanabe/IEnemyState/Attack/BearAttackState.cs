using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAttackState : BaseEnemyState
{
    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorTrigger("Attack");
    }

    public override void Execute(BaseEnemy enemy)
    {
        return;
    }

    public override void Exit(BaseEnemy enemy)
    {
        return;
    }
}
