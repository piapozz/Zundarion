using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BearDecideState : BaseEnemyState
{
    private AnimatorStateInfo stateInfo;
    private EnemyBear enemyBear = null;
    private float _count = 0;

    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Decide", true);
        SetEnemy(enemy);
        enemyBear = enemy.GetComponent<EnemyBear>();
    }

    public override void Execute(BaseEnemy enemy)
    {
        //_count += Time.deltaTime;
        //if (_count < _TRANITION_TIME) return;

        stateInfo = enemy.selfAnimator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("BearDecide")) return;

        Transform playerTransform = CharacterManager.instance.characterList[0].transform;
        float distance = GetDistance(playerTransform);

        // ����������ǂ�������
        if (distance >= _ENEMY_DISTANCE_FAR) { enemy.ChangeState(new BearChasingState()); }
        // �߂�������
        else if (distance <= _ENEMY_DISTANCE_NEAR)
        {
            // �{��|�C���g�����čD��I��������U��
            if (enemyBear.fury >= 50.0f) { enemy.ChangeState(new BearUpperState()); }
            // �T���ڂ�������}���ŉ�����
            else { enemy.ChangeState(new BearJumpState()); }
        }
        // ����������
        else if (distance >= _ENEMY_DISTANCE_NEAR)
        {
            // �D��I�������璼�ڃW�����v�U������
            if (enemyBear.fury >= 50.0f) { enemy.ChangeState(new BearJumpAttackState()); }
            // �ԍ��������
            else { enemy.ChangeState(new BearVigilanceState()); }
        }
    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Decide", false);
    }
}