using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 待機状態
public class BearIdle : IEnemyState
{
    public void Action(EnemyBase.EnemyStatus enemyStatus)
    {
        // もしFound状態にあったらanimatorのFoundフラグを初期化
        if (enemyStatus.m_animator.GetBool("Found") == true)
        {
            // animatorのフラグを切り替え
            enemyStatus.m_animator.SetBool("Found", false);
        }

        // もし敵を見つけたら
        //if ()
        //{
        //    // ステートを切り替え
        //    enemyStatus.m_state = new BearFound();

        //    // 発見アニメーションを再生
        //    enemyStatus.m_animator.SetBool("Found", true);
        //} 
    }
}
