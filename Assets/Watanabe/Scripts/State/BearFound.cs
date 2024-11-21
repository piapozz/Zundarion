using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 見つけた状態の処理

public class BearFound : IEnemyState
{
    float animCount;

    public EnemyBase.EnemyStatus Action(EnemyBase.EnemyStatus enemyStatus)
    {
        // アニメーションが再生されていなかったら
        if (enemyStatus.m_animator.GetBool("Found") == false)
        {
            // アニメーションを再生
            enemyStatus.m_animator.SetBool("Found", true);
            // カウントを初期化
            animCount = 0.0f;
        }

        // アニメーションの現在時間を計算
        animCount = animCount + enemyStatus.m_animatorState.speed * Time.deltaTime;

        // アニメーションの再生が終了したら
        if (enemyStatus.m_animatorState.length < animCount)
        {
            // ステートを切り替える
            enemyStatus.m_state = (int)EnemyBase.EnemyStatus.ActionState.STATE_TRACKING;
        }

        return enemyStatus;
    }
}
