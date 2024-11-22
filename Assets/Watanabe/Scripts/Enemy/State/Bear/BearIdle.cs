using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// �ҋ@���
public class BearIdle : IEnemyState
{
    public EnemyBase.EnemyStatus Action(EnemyBase.EnemyStatus enemyStatus)
    {
        // ����Found��Ԃɂ�������animator��Found�t���O��������
        if (enemyStatus.m_animator.GetBool("Found") == true)
        {
            // animator�̃t���O��؂�ւ�
            enemyStatus.m_animator.SetBool("Found", false);
        }

        // ����
        float direction = enemyStatus.m_relativePosition.magnitude;

        // �����ɂ���čs����ς���
        if (direction < 20.0f)
        {
            // ����
            enemyStatus.m_state = (int)EnemyBase.EnemyStatus.ActionState.STATE_FOUND;
        }

        return enemyStatus;
    }
}
