using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ҋ@���
public class BearIdle : IEnemyState
{
    public void Action(EnemyBase.EnemyStatus enemyStatus)
    {
        // ����Found��Ԃɂ�������animator��Found�t���O��������
        if (enemyStatus.m_animator.GetBool("Found") == true)
        {
            // animator�̃t���O��؂�ւ�
            enemyStatus.m_animator.SetBool("Found", false);
        }

        // �����G����������
        //if ()
        //{
        //    // �X�e�[�g��؂�ւ�
        //    enemyStatus.m_state = new BearFound();

        //    // �����A�j���[�V�������Đ�
        //    enemyStatus.m_animator.SetBool("Found", true);
        //} 
    }
}
