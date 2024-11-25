using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BearTracking : IEnemyState
{
    public EnemyBase.EnemyStatus Action(EnemyBase.EnemyStatus enemyStatus)
    {
        // プレイヤーの方向を取得
        enemyStatus.m_toPlayerAngle = LookAtMe(enemyStatus.m_relativePosition, enemyStatus.m_forward);

        // 距離
        float direction = enemyStatus.m_relativePosition.magnitude;

        // アニメーションが再生されていなかったら
        if (enemyStatus.m_animator.GetBool("Running") == false)
        {
            // アニメーションを再生
            enemyStatus.m_animator.SetBool("Running", true);
        }

        // 距離によって行動を変える
        if (direction > 5.0f && direction < 8.0f)
        {
            // ジャンプで衝撃攻撃
            // enemyStatus.m_state = new BearAttackUnique();
            enemyStatus.m_state = (int)EnemyBase.EnemyStatus.ActionState.STATE_ATTACK_UNIQUE;
        }

        if (direction < 2.0f)
        {
            // 通常攻撃
            enemyStatus.m_state = (int)EnemyBase.EnemyStatus.ActionState.STATE_ATTACK;
        }

        return enemyStatus;
    }

    // プレイヤーの向きを計算
    Quaternion LookAtMe(Vector3 relativePos, Vector3 _forward)
    {
        Quaternion toPlayerAngle;
        // ターゲットの方向への回転
        var lookAtRotation = Quaternion.LookRotation(relativePos, Vector3.up);

        // 回転補正
        var offsetRotation = Quaternion.FromToRotation(_forward, Vector3.forward);

        // 回転補正→ターゲット方向への回転の順に、自身の向きを操作する
        return toPlayerAngle = lookAtRotation * offsetRotation;
    }
}
