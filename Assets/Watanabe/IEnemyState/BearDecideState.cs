using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearDecideState : BaseEnemyState
{
    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Restraint", true);
    }

    public override void Execute(BaseEnemy enemy)
    {
        Transform playerTransform = CharacterManager.instance.characterList[0].transform;
        float distance = enemy.GetRelativePosition(playerTransform).magnitude;

        // ����������ǂ�������
        if(distance >= 20.0f)
        {
            // �ǂ�������
            enemy.ChangeState(new BearChasingState());
        }

        if(distance <= 3.0f)
        {
            // �{��|�C���g�����čD��I��������U��
            if(enemy.GetComponent<EnemyBear>().fury >= 50.0f)
            {
                // �U������
                enemy.ChangeState(new BearUpperState());
            }

            // �T���ڂ�������}���ŉ�����
            else
            {
                // ������
                enemy.ChangeState(new BearHammerState());
            }
        }


        if(distance >= 3.0f)
        {
            // �D��I�������璼�ڃW�����v�U������
            if (enemy.GetComponent<EnemyBear>().fury >= 50.0f)
            {
                // �U������
                enemy.ChangeState(new BearJumpAttackState());
            }

            // �ԍ��������
            else
            {
                // �l�q�����s��
                // enemy.ChangeState(new BearUpperState());
            }
        }

    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Restraint", false);
    }
}