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

    int count = 0;

    // �G��
    public enum StateBear
    {
        STATE_IDLE = 0,               // �ҋ@���
        STATE_FOUND,                  // �������Ƃ�
        STATE_TRACKING,               // ���������ꂽ�Ƃ�
        STATE_TURN,                   // �����͋߂����U���͈͂���O�ꂽ�Ƃ�
        STATE_ATTACK,                 // �ʏ�U��
        STATE_ATTACK_UNIQUE,          // ����̏������ł���U��
        STATE_DEAD,                   // �|���ꂽ�Ƃ�

        MAX
    }
    
    // �G�̍s��
    protected override void UpdateEnemy()
    {
        // �s������
        status.m_state.Action(status);

        // �����ŃX�e�[�g�̕ύX
        // �G���߂�������
        if (1 == 1) status.m_state = new BearAttack();

        


    }

    protected override void Init()
    {
        // ��Ԃ��Ȃ�������ҋ@��Ԃŏ���������
        if (status.m_state == null) status.m_state = new BearIdle();
    }
}
