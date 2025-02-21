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
        // ���C���[�̏����X�V 
        stateInfo = enemy.selfAnimator.GetCurrentAnimatorStateInfo(0);

        // �A�j���[�V�������Đ�����I������������ăX�e�[�g��ύX����
        if (stateInfo.normalizedTime >= 1.0f)
        {
            enemy.ChangeState(new BearWaitState());
        }

    }

    public override void Exit(BaseEnemy enemy)
    {

    }
}
