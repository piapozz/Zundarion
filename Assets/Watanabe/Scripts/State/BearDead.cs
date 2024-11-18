using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearDead : IEnemyState
{
    float animCount;
    public void Action(EnemyBase.EnemyStatus enemyStatus)
    {
        // �A�j���[�V�������Đ�
        enemyStatus.m_animator.SetBool("Dead", true);

        // �A�j���[�V�������Đ�����Ă��Ȃ�������
        if (enemyStatus.m_animator.GetBool("Dead") == false)
        {
            // �A�j���[�V�������Đ�
            enemyStatus.m_animator.SetBool("Dead", true);
            // �J�E���g��������
            animCount = 0.0f;
        }

        // �A�j���[�V�����̌��ݎ��Ԃ��v�Z
        animCount += enemyStatus.m_animatorState.speed * Time.deltaTime;

        // �A�j���[�V�����̍Đ����I�����������
        if (enemyStatus.m_animatorState.length < animCount)
        {
            // ���S�t���O��true�ɐݒ�
            enemyStatus.m_dead = true;
        }
    }
}
