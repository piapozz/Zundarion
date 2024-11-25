using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BearAttackUnique : IEnemyState
{
    float animCount = 0.0f;
    float happenCount = 1.5f;
    bool onceFlag = false;

    CreateCollision.AttackData attackData;

    public EnemyBase.EnemyStatus Action(EnemyBase.EnemyStatus enemyStatus)
    {
        // �A�j���[�V�������Đ�����trigger��L����
        enemyStatus.m_animator.SetTrigger("AttackUnique");

        // �A�j���[�V�����̌��ݎ��Ԃ��v�Z
        animCount += enemyStatus.m_animatorState.speed * Time.deltaTime;

        // �����蔻��̔���
        if(animCount > happenCount && onceFlag != true)
        {
            // AttackData�̐ݒ�
            attackData.position = enemyStatus.m_position;
            attackData.radius = 1.0f;
            attackData.time = 2.0f;
            attackData.layer = enemyStatus.m_collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.ENEMY_ATTACK];
            attackData.tagname = enemyStatus.m_collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_DANGEROUS];

            // �����蔻��̔���
            CreateCollision.instance.CreateCollisionSphere(enemyStatus.m_gameObject, attackData);
            // ��������x�����ɐ�������
            onceFlag = true;
        }

        // �A�j���[�V�����̍Đ����I��������
        if (enemyStatus.m_animatorState.length < animCount)
        {
            // �X�e�[�g��؂�ւ���
            enemyStatus.m_state = (int)EnemyBase.EnemyStatus.ActionState.STATE_TRACKING;
        }

        // �X�V���ꂽ����Ԃ�
        return enemyStatus;
    }
}
