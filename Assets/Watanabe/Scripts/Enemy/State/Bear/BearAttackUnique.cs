using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BearAttackUnique : IEnemyState
{
    float animCount;

    CreateCollision.AttackData attackData;

    public EnemyBase.EnemyStatus Action(EnemyBase.EnemyStatus enemyStatus)
    {
        enemyStatus.m_animator.SetTrigger("AttackUnique");

        // CreateCollision.instance.CreateCollisionSphere(enemyStatus.m_gameObject, attackData);

        // アニメーションの現在時間を計算
        animCount += enemyStatus.m_animatorState.speed * Time.deltaTime;

        // アニメーションの再生が終了したら
        if (enemyStatus.m_animatorState.length < animCount)
        {
            // ステートを切り替える
            enemyStatus.m_state = (int)EnemyBase.EnemyStatus.ActionState.STATE_TRACKING;
        }

        return enemyStatus;
    }
}
