using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// �e�N���X��public���C���X�y�N�^�[��ɕ\��
#if UNITY_EDITOR
[CustomEditor(typeof(EnemyBase))]
#endif

public class EnemyBear : EnemyBase
{
    public enum State
    {
        ENEMY_IDLE = 0,               // �ҋ@���
        ENEMY_FOUND,                  // �������Ƃ�
        ENEMY_TRACKING,               // ���������ꂽ�Ƃ�
        ENEMY_TURN,                   // �����͋߂����U���͈͂���O�ꂽ�Ƃ�
        ENEMY_ATTACK,                 // �ʏ�U��
        ENEMY_ATTACK_UNIQUE,          // ����̏������ł���U��
        ENEMY_DEAD,                   // �|���ꂽ�Ƃ�

        MAX
    }


    // �G�̍s��
    protected override void UpdateEnemy()
    {
        
    }
}
