using System.Collections;
using System.Collections.Generic;
using System.Xml;
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
            if(RandomNumber(2) == 0)
            {
                enemy.ChangeState(new BearUpperState());
            }
            else
            {
                enemy.ChangeState(new BearStrongAttackState());
            }
            return;
        }
    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Chasing", false);
    }
}