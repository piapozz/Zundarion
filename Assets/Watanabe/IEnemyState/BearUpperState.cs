using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearUpperState : BaseEnemyState
{
    private AnimatorStateInfo stateInfo;            // StateInfo

    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorTrigger("Upper");
    }

    public override void Execute(BaseEnemy enemy)
    {
        // レイヤーの情報を更新 
        stateInfo = enemy.selfAnimator.GetCurrentAnimatorStateInfo(0);

        // アニメーションが再生され終わったかを見てステートを変更する
        if (stateInfo.normalizedTime >= 1.0f)
        {
            enemy.ChangeState(new BearWaitState());
        }

    }

    public override void Exit(BaseEnemy enemy)
    {

    }
}
