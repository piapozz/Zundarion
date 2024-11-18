using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearDead : IEnemyState
{
    float animCount;
    public void Action(EnemyBase.EnemyStatus enemyStatus)
    {
        // アニメーションを再生
        enemyStatus.m_animator.SetBool("Dead", true);

        // アニメーションが再生されていなかったら
        if (enemyStatus.m_animator.GetBool("Dead") == false)
        {
            // アニメーションを再生
            enemyStatus.m_animator.SetBool("Dead", true);
            // カウントを初期化
            animCount = 0.0f;
        }

        // アニメーションの現在時間を計算
        animCount += enemyStatus.m_animatorState.speed * Time.deltaTime;

        // アニメーションの再生が終了したら消滅
        if (enemyStatus.m_animatorState.length < animCount)
        {
            // 死亡フラグをtrueに設定
            enemyStatus.m_dead = true;
        }
    }
}
