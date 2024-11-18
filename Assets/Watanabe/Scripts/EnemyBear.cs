using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

// �e�N���X��public���C���X�y�N�^�[��ɕ\��
#if UNITY_EDITOR
[CustomEditor(typeof(EnemyBase))]
#endif

public class EnemyBear : EnemyBase
{
    // �G��
    public enum StateBear
    {
        STATE_IDLE = 0,               // �ҋ@���
        STATE_FOUND,                  // �������Ƃ�
        STATE_TRACKING,               // ���������ꂽ�Ƃ�
        STATE_TURN,                   // �����͋߂����U���͈͂���O�ꂽ�Ƃ�
        STATE_ATTACK,                 // �ʏ�U��
        STATE_ATTACK_UNIQUE,          // ����̏������ł���U��
        STATE_DOWN,                   // �_�E�����
        STATE_DEAD,                   // �|���ꂽ�Ƃ�

        MAX
    }
    
    // �G�̍s��
    // ���_���[�W�������Ō�ɂ��Ȃ��ƃX�e�[�g���X�V����Đ����Ԃ����肷��
    protected override void UpdateEnemy()
    {
        // Ray���΂���Player�^�O���������̂̏����擾
        // GetPlayerObject();

        // �s������
        status.m_state.Action(status);

        // �u���C�N�l�����܂�����
        if (status.m_breakMax <= status.m_break)
        {
            // ��Ԃ̐؂�ւ�
            status.m_state = new BearDown();
        }

        // �_���[�W����
        if(status.m_health <= 0)
        {
            status.m_state = new BearDead();
        }

        // �A�j���[�V�����̍Đ����I������玩�g������
        if (status.m_dead == true) Destroy(this.gameObject);
    }

    protected override void Init()
    {
        // ��Ԃ��Ȃ�������ҋ@��Ԃŏ���������
        if (status.m_state == null) status.m_state = new BearIdle();
    }
}
