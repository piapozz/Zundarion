using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestraintState : StateMachineBehaviour
{
    private BaseEnemy enemy = null;
    private float animCount;
    private bool enterFlag = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<BaseEnemy>();
        animCount = 0.0f;
        Debug.Log("aaa");

        enterFlag = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(enterFlag)
        {
            animCount += stateInfo.speedMultiplier * Time.deltaTime;

            // ‘Ò‹@ŽžŠÔ
            if (2.0f <= animCount)
            {
                enemy.Restraint();
                enterFlag = false;
                animCount = 0.0f;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
