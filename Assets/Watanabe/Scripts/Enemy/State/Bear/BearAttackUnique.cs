using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BearAttackUnique : IEnemyState
{
    float animCount;

    CreateCollision.AttackData attackData;

    public EnemyBase.EnemyStatus Action(EnemyBase.EnemyStatus enemyStatus)
    {
        enemyStatus.m_animator.SetTrigger("AttackUnique");

        // CreateCollision.instance.CreateCollisionSphere(enemyStatus.m_gameObject, attackData);

        // �A�j���[�V�����̌��ݎ��Ԃ��v�Z
        animCount += enemyStatus.m_animatorState.speed * Time.deltaTime;

        // �A�j���[�V�����̍Đ����I��������
        if (enemyStatus.m_animatorState.length < animCount)
        {
            // �X�e�[�g��؂�ւ���
            enemyStatus.m_state = (int)EnemyBase.EnemyStatus.ActionState.STATE_TRACKING;
        }

        return enemyStatus;
    }
}
