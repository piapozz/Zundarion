using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �G�N���X�̐e
// ��b�X�e�[�^�X�Ǝg�p�����AI��ScriptableObject�ł������悤�ɍ��
// UpdateEnemy�֐����q�N���X�ŕҏW����AI�����

public abstract class EnemyBase : MonoBehaviour
{
    public InitialStatus initialStatus;             // ScriptableObject�̏��

    // �G�̃X�e�[�^�X
    public struct EnemyStatus
    {
        public int m_enemyNum;              // �G�̔ԍ�

        public float m_health;              // �w���X
        public float m_speed;               // �X�s�[�h
        public float m_power;               // �p���[ 
        public float m_break;               // �u���C�N�l
        public float m_distance;            // �G�̎��F�͈�
        public float m_vision;              // �G�̎���
        public Vector3 m_position;          // �G�̍��W
        public Vector3 m_positionNext;      // �G�̈ړ���\��̍��W

        public IEnemyState m_state;         // �G�̏��
        public Animator m_animator;           // �L�����N�^�[�Ɏg���Ă���Animator
    }

    protected EnemyStatus status;

    // �G�̍s��
    protected abstract void UpdateEnemy();

    // �󋵂ɉ����ăX�e�[�^�X�ύX����
    protected abstract void Init();

    // �����ݒ�
    private void Awake()
    {
        // ScriptableObject����X�e�[�^�X���擾
        InputStatus();
        // �G�̎�ނɉ����ĕK�v�ȏ����������s
        Init();
        // �A�j���[�^�[�̏�����
        status.m_animator = gameObject.GetComponent<Animator>();
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
        status.m_health = initialStatus.health;
        status.m_power = initialStatus.power;
        status.m_speed = initialStatus.speed;
    }

    // �A�j���[�V�����ōX�V���ꂽ�I�u�W�F�N�g�̍��W��ۑ�
    private void PositionUpdate()
    {
        status.m_position = gameObject.transform.position;
    }

    private float GetDamage(float damageSize) { return damageSize / 2.0f; }
}
