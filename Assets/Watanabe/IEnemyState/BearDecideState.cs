using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BearDecideState : BaseEnemyState
{
    private AnimatorStateInfo stateInfo;
    private EnemyBear enemyBear = null;
    private Transform playerTransform = null;
    private float _count = 0;

    private float speed = 1.0f;

    private readonly float _ENEMY_ROTATE_SPEED = 1.0f; 
    private readonly float _ENEMY_VIGILANCE_TIME = 3.0f;
    private readonly int _ENEMY_FURY_COST_UPPER = -15;
    private readonly int _ENEMY_FURY_COST_JUMPATTACK = -15;

    private enum NearAttack
    {
        ATTACK,
        STRONG_ATTACK,
        UPPER,

        MAX
    }

    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Decide", true);
        enemyBear = enemy.GetComponent<EnemyBear>();
        SetEnemy(enemy);
        playerTransform = player.transform;

    }

    public override void Execute(BaseEnemy enemy)
    {
        stateInfo = enemy.selfAnimator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("BearDecide")) return;

        _count += Time.deltaTime;
        float distance = GetDistance(playerTransform);
        Vector3 targetVec = enemy.GetTargetVec(playerTransform.position);
        enemy.Rotate(targetVec, _ENEMY_ROTATE_SPEED);

        if (distance <= _ENEMY_DISTANCE) { enemy.Move(speed, Vector3.right); }
        else { enemy.Move(speed, Vector3.forward); }
        
        // ���f�O�܂łɂ��Ȃ�߂Â��Ă��Ă����烉���_���ŋߋ����U���Ɉڍs����
        if (distance <= _ENEMY_DISTANCE_NEAR / 2) 
        {
            int randomNearAttackID = RandomNumber((int)NearAttack.MAX);
            if (randomNearAttackID == (int)NearAttack.ATTACK) { enemy.ChangeState(new BearAttackState()); }
            else if (randomNearAttackID == (int)NearAttack.STRONG_ATTACK) { enemy.ChangeState(new BearStrongAttackState()); }
            else enemy.ChangeState(new BearUpperState());

            return;
        }

        // �����ɂ���Ďv�l���Ԃ�ς���
        if (_count < distance) return;

        // ����������
        if (distance >= _ENEMY_DISTANCE_NEAR)
        {
            // �D��I�������璼�ڃW�����v�U������
            if (enemyBear.GetFury() >= _ENEMY_FURY_COST_JUMPATTACK)
            {
                enemy.ChangeState(new BearJumpAttackState());
                enemyBear.ChangeFury(_ENEMY_FURY_COST_JUMPATTACK);
            }
            // �ԍ��������
            else { enemy.ChangeState(new BearVigilanceState()); }
        }

        // ����������ǂ�������
        if (distance >= _ENEMY_DISTANCE_FAR) { enemy.ChangeState(new BearChasingState()); }
        // �߂�������
        else if (distance <= _ENEMY_DISTANCE_NEAR)
        {
            // �{��|�C���g�����čD��I��������U��
            if (enemyBear.GetFury() >= _ENEMY_FURY_COST_UPPER) 
            {
                enemy.ChangeState(new BearUpperState());
                enemyBear.ChangeFury(_ENEMY_FURY_COST_UPPER);
            }
            // �T���ڂ�������}���ŉ�����
            else { enemy.ChangeState(new BearTackleState()); }
        }
    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Decide", false);
    }
}