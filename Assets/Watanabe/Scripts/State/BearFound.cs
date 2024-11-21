using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��������Ԃ̏���

public class BearFound : IEnemyState
{
    float animCount;

    public EnemyBase.EnemyStatus Action(EnemyBase.EnemyStatus enemyStatus)
    {
        // �A�j���[�V�������Đ�����Ă��Ȃ�������
        if (enemyStatus.m_animator.GetBool("Found") == false)
        {
            // �A�j���[�V�������Đ�
            enemyStatus.m_animator.SetBool("Found", true);
            // �J�E���g��������
            animCount = 0.0f;
        }

        // �A�j���[�V�����̌��ݎ��Ԃ��v�Z
        animCount = animCount + enemyStatus.m_animatorState.speed * Time.deltaTime;

        // �A�j���[�V�����̍Đ����I��������
        if (enemyStatus.m_animatorState.length < animCount)
        {
            // �X�e�[�g��؂�ւ���
            enemyStatus.m_state = (int)EnemyBase.EnemyStatus.ActionState.STATE_TRACKING;
        }

        return enemyStatus;
    }
}
