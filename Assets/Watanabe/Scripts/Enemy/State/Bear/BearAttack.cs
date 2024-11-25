using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BearAttack : IEnemyState
{
    float animCount;
    float happenCount = 0.2f;
    bool onceFlag = false;

    CreateCollision.AttackData attackData;

    public EnemyBase.EnemyStatus Action(EnemyBase.EnemyStatus enemyStatus)
    {
        enemyStatus.m_animator.SetTrigger("Attack");

        // アニメーションの現在時間を計算
        animCount += enemyStatus.m_animatorState.speed * Time.deltaTime;

        // 当たり判定の発生
        if (animCount > happenCount && onceFlag != true)
        {
            // AttackDataの設定
            attackData.position = enemyStatus.m_position;
            attackData.radius = 1.0f;
            attackData.time = 2.0f;
            attackData.layer = enemyStatus.m_collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.ENEMY_ATTACK];
            attackData.tagname = enemyStatus.m_collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_NOMAL];

            // 当たり判定の発生
            CreateCollision.instance.CreateCollisionSphere(enemyStatus.m_gameObject, attackData);
            // 発生を一度だけに制限する
            onceFlag = true;
        }

        // アニメーションの再生が終了したら
        if (enemyStatus.m_animatorState.length < animCount)
        {
            // ステートを切り替える
            enemyStatus.m_state = (int)EnemyBase.EnemyStatus.ActionState.STATE_TRACKING;
        }

        return enemyStatus;
    }
}
