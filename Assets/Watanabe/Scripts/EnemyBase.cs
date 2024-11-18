using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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
        public float m_defence;             // �h���
        public float m_break;               // ���݂̃u���C�N�l
        public float m_breakMax;            // �u���C�N�l�̍ő�l
        public float m_multiplier;          // �u���C�N������̃_���[�W�{��
        public float m_distance;            // �G�̎��F�͈�
        public float m_vision;              // �G�̎���
        public bool m_dead;                 // ���S���
        public Vector3 m_position;          // �G�̍��W
        public Vector3 m_positionNext;      // �G�̈ړ���\��̍��W

        public IEnemyState m_state;                 // �G�̏��
        public GameObject m_gameObject;             // �Q�[���I�u�W�F�N�g
        public GameObject m_playerObject;           // Player�̃f�[�^
        public Animator m_animator;                 // �L�����N�^�[�Ɏg���Ă���Animator
        public AnimatorStateInfo m_animatorState;   // �A�j���[�V�����Đ����Ԃ̒������擾���邽�߂̕ϐ�

        public enum ActionState
        {
            STATE_IDLE = 0,                 // �ҋ@���
            STATE_FOUND,                    // �������Ƃ�
            STATE_TRACKING,                 // ���������ꂽ�Ƃ�
            STATE_TURN,                     // �����͋߂����U���͈͂���O�ꂽ�Ƃ�
            STATE_ATTACK,                   // �ʏ�U��
            STATE_ATTACK_UNIQUE,            // ����̏������ł���U��
            STATE_DOWN,                     // �_�E�����
            STATE_DEAD,                     // �|���ꂽ�Ƃ�

            MAX
        }
    }

    //public float visionDistance = 10.0f;  // ���E�̋���
    //public float visionAngle = 45.0f;     // ����p
    //public int rayCount = 50;             // ��`�ɔ�΂�Ray�̖{��

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
        // �����蔻��̐����Ɏg��GameObject��������
        status.m_gameObject = this.gameObject;
        // �v���C���[���擾 ��GameObject.Find()�͏d���炵���̂Ŏg���Ȃ珉�����Ȃǂ̃^�C�~���O�ňꊇ
        status.m_playerObject = GameObject.Find("Player");
    }

    // �G�̏���
    private void Update()
    {
        // �A�j���[�V�����̏�Ԃ��擾
        status.m_animatorState = status.m_animator.GetCurrentAnimatorStateInfo(0);
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
        status.m_break = initialStatus.breakMax;
    }

    /*
    // Ray���΂��Ď�����쐬
    protected void GetPlayerObject()
    {
        // Ray���Ƃ̊p�x
        float angleStep = (visionAngle * 2) / (rayCount - 1);
        // ��`�̍��[����J�n
        float startAngle = -visionAngle;

        // �ݒ肳�ꂽ�񐔂���Ray�o��
        for (int i = 0; i < rayCount; i++)
        {
            // ���݂̊p�x���v�Z
            float currentAngle = startAngle + (angleStep * i);

            // �p�x�Ɋ�Â���Ray�̕������v�Z
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward;

            // Ray���΂�
            Ray enemyRay = new Ray(transform.position, direction);

            // ����
            if (Physics.Raycast(enemyRay, out RaycastHit hit, visionDistance))
            {
                Debug.Log("���������I");

                // �^�O�̊m�F
                if (hit.collider.CompareTag("Player"))
                {
                    // �^�O���Q�[���I�u�W�F�N�g��������i�[
                    status.m_playerObject = hit.collider.gameObject;
                }
            }
            else
            {
                // Ray���Ώۂɓ�����Ȃ��ꍇ
                Debug.DrawRay(transform.position, direction * visionDistance, Color.red);
            }
        }
    }
    */

    // �A�j���[�V�����ōX�V���ꂽ�I�u�W�F�N�g�̍��W��ۑ�
    private void PositionUpdate()
    {
        status.m_position = gameObject.transform.position;
    }

    // �_���[�W���󂯂�
    public void GetDamage(float damageSize) { status.m_health -= damageSize * status.m_defence * status.m_multiplier; }

    // �u���C�N�l��ϓ�������
    public void GetBreakPoint(float breakSize) { status.m_break -= breakSize; }
}
