using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BearIdleState : BaseEnemyState
{
    private AnimatorStateInfo stateInfo;            // StateInfo
    private float _count = 0;
    private Vector3 targetVec;
    private Transform playerTransform;

    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Idle", true);
        SetEnemy(enemy);
        playerTransform = SetTransform();
    }

    public override void Execute(BaseEnemy enemy)
    {
        if (playerTransform == null) return;
        targetVec = GetTargetVec(playerTransform);

        enemy.Rotate(targetVec);

        _count += Time.deltaTime;
        if (_count < 1) return;
        if(enemy is EnemyBear)
        {
            enemy.ChangeState(new BearDecideState());
        }
        else if (enemy is EnemyHumanRobot)
        {
            enemy.ChangeState(new HumanRobotDecideState());
        }

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
        enemy.SetAnimatorBool("Idle", false);
    }
}
