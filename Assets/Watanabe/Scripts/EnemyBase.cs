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

    private Animator animator;                      // �g�p����animator

    // �G�̃X�e�[�^�X
    public struct Status
    {
        public int m_enemyNum;

        public float m_health;
        public float m_speed;
        public float m_strength;
        public float m_break;
        public Vector3 m_position;
        public Vector3 m_positionNext;
    }

    public Status status;

    public enum Patterns
    {
        NEUTRAL = 0,
        ATTACK,

        MAX
    }

    // �G�̍s��
    protected abstract void UpdateEnemy();

    // �����ݒ�
    private void Awake()
    {
        InputStatus();
        animator = gameObject.GetComponent<Animator>();
    }

    // �G�̏���
    private void Update()
    {
        // AI�̋���
        UpdateEnemy();
    }

    // �X�e�[�^�X�̏�����
    private void InputStatus()
    {
        // ScriptableObject�̏���ǂݍ���
        status.m_health = enemyStatus.health;
        status.m_strength = enemyStatus.strength;
        status.m_speed = enemyStatus.speed;
    }

    // �A�j���[�V�����ōX�V���ꂽ�I�u�W�F�N�g�̍��W��ۑ�
    private void PositionUpdate()
    {
        status.m_position = gameObject.transform.position;
    }

    private float GetDamage(float damageSize) { return damageSize / 2.0f; }
}
