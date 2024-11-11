using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// �G�N���X�̐e
// ��b�X�e�[�^�X�Ǝg�p�����AI��ScriptableObject�ł������悤�ɍ��
// UpdateEnemy�֐����q�N���X�ŕҏW����AI�����

public abstract class EnemyBase : MonoBehaviour
{
    public EnemyStatus enemyStatus;                 // ScriptableObject�̏��
    public GameObject instance;

    // �G�̃X�e�[�^�X
    public struct Status
    {
        public int m_enemyNum;
        public float m_health;
        public float m_speed;
        public float m_strength;
    }

    public enum Patterns
    {
        NEUTRAL = 0,
        ATTACK,

        MAX
    }

    // �G�̍s��
    protected abstract void UpdateEnemy();

    private void Awake()
    {
        Type aiComponentType = Type.GetType(enemyStatus.aiName);
        EnemyBase aiComponent = instance.AddComponent(aiComponentType) as EnemyBase;


    }



    // �G�̏���
    private void Update()
    {
        UpdateEnemy();
    }
}
