using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearDecideState : BaseEnemyState
{
    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Decide", true);
        SetEnemy(enemy);
    }

    public override void Execute(BaseEnemy enemy)
    {
        Transform playerTransform = CharacterManager.instance.characterList[0].transform;
        float distance = enemy.GetRelativePosition(playerTransform).magnitude;

        // 遠すぎたら追いかける
        if(GetDistance(playerTransform) >= 20.0f)
        {
            // 追いかける
            enemy.ChangeState(new BearChasingState());
        }

        if(GetDistance(playerTransform) <= 5.0f)
        {
            // 怒りポイントを見て好戦的だったら攻撃
            if(enemy.GetComponent<EnemyBear>().fury >= 50.0f)
            {
                // 攻撃する
                enemy.ChangeState(new BearUpperState());
            }

            // 控え目だったら急速で下がる
            else
            {
                // 下がる
                enemy.ChangeState(new BearJumpState());
            }
        }

        if(GetDistance(playerTransform) >= 10.0f)
        {
            // 好戦的だったら直接ジャンプ攻撃する
            if (enemy.GetComponent<EnemyBear>().fury >= 50.0f)
            {
                // 攻撃する
                enemy.ChangeState(new BearJumpAttackState());
            }

            // 間合いを取る
            else
            {
                // 様子見を行う
                enemy.ChangeState(new BearVigilanceState());
            }
        }

    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Decide", false);
    }
}