using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BearIdleState : BaseEnemyState
{
    private AnimatorStateInfo stateInfo;            // StateInfo
    float count = 0;

    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Idle", true);
    }

    public override void Execute(BaseEnemy enemy)
    {
        count += Time.deltaTime;

        if (count < 1) return;

        enemy.ChangeState(new BearDecideState());

        //// ���C���[�̏����X�V 
        //stateInfo = enemy.selfAnimator.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(stateInfo.normalizedTime);
        //// �A�j���[�V�������Đ�����I������������ăX�e�[�g��ύX����
        //if (stateInfo.normalizedTime >= 0.2f)
        //{
        //    enemy.ChangeState(new BearDecideState());
        //}
    }

    public override void Exit(BaseEnemy enemy)
    {

    }
}
