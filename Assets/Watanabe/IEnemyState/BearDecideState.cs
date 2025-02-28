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

    private float speed = 1.0f;
    private readonly float _ENEMY_VIGILANCE_TIME = 3.0f;

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
        SetEnemy(enemy);
        enemyBear = enemy.GetComponent<EnemyBear>();
    }

    public override void Execute(BaseEnemy enemy)
    {
        stateInfo = enemy.selfAnimator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("BearDecide")) return;

        _count += Time.deltaTime;
        Transform playerTransform = CharacterManager.instance.characterList[0].transform;
        float distance = GetDistance(playerTransform);
        Vector3 targetVec = enemy.GetTargetVec(playerTransform.position);
        enemy.Rotate(targetVec);

        if (distance <= _ENEMY_DISTANCE) { enemy.Move(speed, Vector3.right); }
        else { enemy.Move(speed, Vector3.forward); }
        
        // ���f�O�܂łɂ��Ȃ�߂Â��Ă��Ă����烉���_���ŋߋ����U���Ɉڍs����
        if (distance <= _ENEMY_DISTANCE_NEAR / 2) 
        {
            int randomNearAttackID = RandomNumber((int)NearAttack.MAX);
            Debug.Log(randomNearAttackID);
            if (randomNearAttackID == (int)NearAttack.ATTACK) { enemy.ChangeState(new BearAttackState()); }
            else if (randomNearAttackID == (int)NearAttack.STRONG_ATTACK) { enemy.ChangeState(new BearStrongAttackState()); }
            else enemy.ChangeState(new BearUpperState());

            return;
        }

        // �v�l���Ԃ��߂��Ă��Ȃ������烋�[�v
        if (_count < _ENEMY_VIGILANCE_TIME) return;

        // ����������ǂ�������
        if (distance >= _ENEMY_DISTANCE_FAR) { enemy.ChangeState(new BearChasingState()); }
        // �߂�������
        else if (distance <= _ENEMY_DISTANCE_NEAR)
        {
            // �{��|�C���g�����čD��I��������U��
            if (enemyBear.fury >= 50.0f) { enemy.ChangeState(new BearUpperState()); }
            // �T���ڂ�������}���ŉ�����
            else { enemy.ChangeState(new BearTackleState()); }
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