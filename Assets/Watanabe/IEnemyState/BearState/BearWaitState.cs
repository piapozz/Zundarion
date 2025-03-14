using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BearWaitState : BaseEnemyState
{
    private float _count = 0;
    private AnimatorStateInfo stateInfo;

    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Wait", true);
    }

    public override void Execute(BaseEnemy enemy)
    {
        //_count += Time.deltaTime;
        //if (_count < _TRANITION_TIME) return;

        stateInfo = enemy.selfAnimator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("BearWait")) return;

        enemy.ChangeState(new BearIdleState());
    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Wait", false);
    }
}
