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
        playerTransform = SetTransform();

    }

    public override void Execute(BaseEnemy enemy)
    {
        stateInfo = enemy.selfAnimator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("BearDecide")) return;

        if (playerTransform == null) return;
        _count += Time.deltaTime;
        float distance = GetDistance(playerTransform);
        Vector3 targetVec = enemy.GetTargetVec(playerTransform.position);
        enemy.Rotate(targetVec);

        if (distance <= _ENEMY_DISTANCE) { enemy.Move(speed, Vector3.right); }
        else { enemy.Move(speed, Vector3.forward); }
        
        // 判断前までにかなり近づいてきていたらランダムで近距離攻撃に移行する
        if (distance <= _ENEMY_DISTANCE_NEAR / 2) 
        {
            int randomNearAttackID = RandomNumber((int)NearAttack.MAX);
            if (randomNearAttackID == (int)NearAttack.ATTACK) { enemy.ChangeState(new BearAttackState()); }
            else if (randomNearAttackID == (int)NearAttack.STRONG_ATTACK) { enemy.ChangeState(new BearStrongAttackState()); }
            else enemy.ChangeState(new BearUpperState());

            return;
        }

        // 距離によって思考時間を変える
        if (_count < distance) return;

        // 遠かったら
        if (distance >= _ENEMY_DISTANCE_NEAR)
        {
            // 好戦的だったら直接ジャンプ攻撃する
            if (enemyBear.GetFury() >= _ENEMY_FURY_COST_JUMPATTACK)
            {
                enemy.ChangeState(new BearJumpAttackState());
                enemyBear.ChangeFury(_ENEMY_FURY_COST_JUMPATTACK);
            }
            // 間合いを取る
            else { enemy.ChangeState(new BearVigilanceState()); }
        }

        // 遠すぎたら追いかける
        if (distance >= _ENEMY_DISTANCE_FAR) { enemy.ChangeState(new BearChasingState()); }
        // 近かったら
        else if (distance <= _ENEMY_DISTANCE_NEAR)
        {
            // 怒りポイントを見て好戦的だったら攻撃
            if (enemyBear.GetFury() >= _ENEMY_FURY_COST_UPPER) 
            {
                enemy.ChangeState(new BearUpperState());
                enemyBear.ChangeFury(_ENEMY_FURY_COST_UPPER);
            }
            // 控え目だったら急速で下がる
            else { enemy.ChangeState(new BearTackleState()); }
        }
    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Decide", false);
    }
}