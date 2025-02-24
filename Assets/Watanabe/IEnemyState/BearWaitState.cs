using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BearWaitState : BaseEnemyState
{
    float count = 0;

    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Wait", true);
    }

    public override void Execute(BaseEnemy enemy)
    {
        count += Time.deltaTime;

        if (count < 1) return;

        enemy.ChangeState(new BearIdleState());
    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Wait", false);
    }
}
