using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearWaitState : BaseEnemyState
{
    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Wait", true);
    }

    public override void Execute(BaseEnemy enemy)
    {
        enemy.ChangeState(new BearIdleState());
    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Wait", false);
    }
}
