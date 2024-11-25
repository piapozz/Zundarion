using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BearTracking : IEnemyState
{
    public EnemyBase.EnemyStatus Action(EnemyBase.EnemyStatus enemyStatus)
    {
        // �v���C���[�̕������擾
        enemyStatus.m_toPlayerAngle = LookAtMe(enemyStatus.m_relativePosition, enemyStatus.m_forward);

        // ����
        float direction = enemyStatus.m_relativePosition.magnitude;

        // �A�j���[�V�������Đ�����Ă��Ȃ�������
        if (enemyStatus.m_animator.GetBool("Running") == false)
        {
            // �A�j���[�V�������Đ�
            enemyStatus.m_animator.SetBool("Running", true);
        }

        // �����ɂ���čs����ς���
        if (direction > 5.0f && direction < 8.0f)
        {
            // �W�����v�ŏՌ��U��
            // enemyStatus.m_state = new BearAttackUnique();
            enemyStatus.m_state = (int)EnemyBase.EnemyStatus.ActionState.STATE_ATTACK_UNIQUE;
        }

        if (direction < 2.0f)
        {
            // �ʏ�U��
            enemyStatus.m_state = (int)EnemyBase.EnemyStatus.ActionState.STATE_ATTACK;
        }

        return enemyStatus;
    }

    // �v���C���[�̌������v�Z
    Quaternion LookAtMe(Vector3 relativePos, Vector3 _forward)
    {
        Quaternion toPlayerAngle;
        // �^�[�Q�b�g�̕����ւ̉�]
        var lookAtRotation = Quaternion.LookRotation(relativePos, Vector3.up);

        // ��]�␳
        var offsetRotation = Quaternion.FromToRotation(_forward, Vector3.forward);

        // ��]�␳���^�[�Q�b�g�����ւ̉�]�̏��ɁA���g�̌����𑀍삷��
        return toPlayerAngle = lookAtRotation * offsetRotation;
    }
}
