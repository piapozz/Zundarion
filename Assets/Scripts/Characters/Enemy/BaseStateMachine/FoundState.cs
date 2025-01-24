using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundState : StateMachineBehaviour
{
    private BaseEnemy enemy = null;

    float animCount;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<BaseEnemy>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animCount += stateInfo.speedMultiplier * Time.deltaTime;

        if (stateInfo.length <= animCount) enemy.SetAnimatorBool("Restraint", true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
