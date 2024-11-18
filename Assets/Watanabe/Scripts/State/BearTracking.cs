using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BearTracking : IEnemyState
{
    public void Action(EnemyBase.EnemyStatus enemyStatus)
    {
        // アニメーションが再生されていなかったら
        if (enemyStatus.m_animator.GetBool("Running") == false)
        {
            // アニメーションを再生
            enemyStatus.m_animator.SetBool("Running", true);
        }

        // プレイヤーとの距離を測って行動する
        // TargetRelativePosition = enemyStatus.m_playerObject.transform.position - enemyStatus.m_position;
    }
}
