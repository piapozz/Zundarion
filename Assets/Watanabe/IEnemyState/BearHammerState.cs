using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearHammerState : BaseEnemyState
{
    private AnimatorStateInfo stateInfo;            // StateInfo

    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorTrigger("Hammer");
    }

    public override void Execute(BaseEnemy enemy)
    {
        // ���C���[�̏����X�V 
        stateInfo = enemy.selfAnimator.GetCurrentAnimatorStateInfo(0);

        // Debug.Log(stateInfo.normalizedTime);
        Debug.Log(stateInfo.normalizedTime * stateInfo.length);

        // �A�j���[�V�������Đ�����I������������ăX�e�[�g��ύX����
        if (stateInfo.normalizedTime * stateInfo.length >= stateInfo.length)
        {

        }

    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.ChangeState(new BearWaitState());
    }
}
