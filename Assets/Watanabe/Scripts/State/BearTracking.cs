using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BearTracking : IEnemyState
{
    public void Action(EnemyBase.EnemyStatus enemyStatus)
    {
        // �A�j���[�V�������Đ�����Ă��Ȃ�������
        if (enemyStatus.m_animator.GetBool("Running") == false)
        {
            // �A�j���[�V�������Đ�
            enemyStatus.m_animator.SetBool("Running", true);
        }

        // �v���C���[�Ƃ̋����𑪂��čs������
        // TargetRelativePosition = enemyStatus.m_playerObject.transform.position - enemyStatus.m_position;
    }
}
