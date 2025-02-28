using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineStun : StateMachineBehaviour
{
    private BaseEnemy enemy = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<BaseEnemy>();
        enemy.ChangeState(new BearHitBackState());
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(stateInfo.normalizedTime);

        // アニメーションが再生され終わったかを見てステートを変更する
        if (stateInfo.normalizedTime >= 1.0f)
        {
            enemy.ChangeState(new BearIdleState());
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
