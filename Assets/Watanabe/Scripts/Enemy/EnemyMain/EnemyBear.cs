using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using static EnemyBase.EnemyStatus;
using Unity.VisualScripting;

// �e�N���X��public���C���X�y�N�^�[��ɕ\��
#if UNITY_EDITOR
[CustomEditor(typeof(EnemyBase))]
#endif

public class EnemyBear : EnemyBase
{    
    // �G�̍s��
    // ���_���[�W�������Ō�ɂ��Ȃ��ƃX�e�[�g���X�V����Đ����Ԃ����肷��
    protected override void UpdateEnemy()
    {
        // Ray���΂���Player�^�O���������̂̏����擾
        // GetPlayerObject();

        // �s������
        status = actionState.Action(status);

        // �v���C���[�̕�������
        gameObject.transform.rotation = status.m_toPlayerAngle;
        
        // Eif(transform.position.y < 0.0f) transform.position = status.m_position;

        // �X�e�[�g�̕ύX������΃X�e�[�g��ύX
        if (oldState != status.m_state) SetState(status.m_state);

        // �u���C�N�l�����܂�����
        if (status.m_breakMax <= status.m_break)
        {
            // ��Ԃ̐؂�ւ�
            // status.m_state = new BearDown();
        }

        // �_���[�W����
        if(status.m_health <= 0 )
        {
            status.m_state = (int)EnemyBase.EnemyStatus.ActionState.STATE_DEAD;
        }

        // �A�j���[�V�����̍Đ����I������玩�g������
        if (status.m_dead == true) Destroy(this.gameObject);
    }

    protected override void Init()
    {
        // ��Ԃ��Ȃ�������ҋ@��Ԃŏ���������
        if (actionState == null) actionState = new BearIdle();
    }
}
