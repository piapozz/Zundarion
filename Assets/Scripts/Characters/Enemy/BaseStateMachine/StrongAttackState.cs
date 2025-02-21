using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttackState : StateMachineBehaviour
{
    private BaseEnemy enemy = null;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<BaseEnemy>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.EnemyAction();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}






