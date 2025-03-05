using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class BearChasingState : BaseEnemyState
{
    private float _count = 0;
    private Vector3 targetVec;
    private Transform playerTransform = null;

    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Chasing", true);
        SetEnemy(enemy);
        playerTransform = player.transform;
    }

    public override void Execute(BaseEnemy enemy)
    {
        targetVec = GetTargetVec(playerTransform);

        enemy.Move(enemy.speed);
        enemy.Rotate(targetVec);

        _count += Time.deltaTime;
        if (_count < _TRANITION_TIME) return;

        float distance = GetDistance(playerTransform);
        if (distance <= _ENEMY_DISTANCE_NEAR)
        {
            if (RandomNumber(2) == 1)
            {
                enemy.ChangeState(new BearUpperState());
            }
            else
            {
                enemy.ChangeState(new BearStrongAttackState());
            }
        }
    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Chasing", false);
    }
}