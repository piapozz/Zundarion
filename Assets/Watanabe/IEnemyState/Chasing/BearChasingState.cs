using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearChasingState : BaseEnemyState
{
    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Chasing", true);
    }

    public override void Execute(BaseEnemy enemy)
    {
        enemy.Move(enemy.speed);
        enemy.Rotate(enemy.targetVec);
        if (enemy.GetRelativePosition(enemy.player.transform).magnitude <= 2.0f)
        {
            enemy.ChangeState(new BearRestraintState());
            return;
        }
    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Chasing", false);
    }
}