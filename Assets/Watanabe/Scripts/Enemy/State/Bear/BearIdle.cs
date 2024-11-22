using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 待機状態
public class BearIdle : IEnemyState
{
    public EnemyBase.EnemyStatus Action(EnemyBase.EnemyStatus enemyStatus)
    {
        // もしFound状態にあったらanimatorのFoundフラグを初期化
        if (enemyStatus.m_animator.GetBool("Found") == true)
        {
            // animatorのフラグを切り替え
            enemyStatus.m_animator.SetBool("Found", false);
        }

        // 距離
        float direction = enemyStatus.m_relativePosition.magnitude;

        // 距離によって行動を変える
        if (direction < 20.0f)
        {
            // 発見
            enemyStatus.m_state = (int)EnemyBase.EnemyStatus.ActionState.STATE_FOUND;
        }

        return enemyStatus;
    }
}
