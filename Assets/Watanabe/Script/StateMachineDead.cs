using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineDead : StateMachineBehaviour
{
    private BaseEnemy enemy = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<BaseEnemy>();
        
        // �X�e�[�g�J��
        enemy.ChangeState(new BearDeadState());

        // �����蔻�薳����
        enemy.GetComponent<CapsuleCollider>().enabled = false;
        Rigidbody rigidbody = enemy.GetComponent<Rigidbody>();
        Destroy(rigidbody);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}
