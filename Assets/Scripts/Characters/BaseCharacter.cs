/*
 * @file BaseCharacter.cs
 * @brief �L�����N�^�[���ʂ̏��⏈�����܂Ƃ߂��x�[�X�N���X
 * @author sein
 * @date 2025/1/17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

// �e�N���X��public���C���X�y�N�^�[��ɕ\��
#if UNITY_EDITOR
[CustomEditor(typeof(BaseCharacter))]
#endif

public abstract class BaseCharacter : MonoBehaviour
{
    [SerializeField] CharacterData charaData;       // �L�����N�^�[�̃f�[�^

    /// <summary>�v���C���[�̈ړ��f�[�^</summary>
    [SerializeField]
    private CollisionAction _collisionAction = null;

    // �L�����N�^�[�̃X�e�[�^�X
    protected float healthMax;         // �ő�̗�
    protected float health;            // ���݂̗̑�
    protected float strength;          // �U����
    protected float defence;           // �h���
    protected float speed;             // ����

    protected Vector3 position;        // ���W
    protected float multiplier;        // �{��

    /// <summary>
    /// ScriptableObject���g���ď�����
    /// </summary>
    private void Awake()
    {
        healthMax = charaData.health;
        health = healthMax;
        strength = charaData.strength;
        defence = charaData.defence;
        speed = charaData.speed;
    }

    /// <summary>
    /// �_���[�W���󂯂鏈��
    /// </summary>
    /// <param name="damageSize"></param>
    public void TakeDamage(float damageSize)
    {
        float damage = damageSize * defence * multiplier;

        if (damage > healthMax / 3)
        {
            // ��:�傫���̂�����A�j���[�V�������Đ�
        }
        else if (damage > healthMax / 100)
        {
            // ��:�̂�����A�j���[�V�������Đ�
        }

        // ���̐��ɂȂ�Ȃ��悤�ɕ␳
        health = Mathf.Max(0, health - damage);

        health -= damage;
    }

    public float GetCharacterHealth() {  return health; }

    public void CreateCollisionEvent(CollisionAction.CollisionLayer layer, Vector3 genOffset, float radius, float damage)
    {
        // �����蔻��𐶐�����
        CreateCollision.AttackData data = new CreateCollision.AttackData().zero();

        data.tagname = transform.tag;
        data.layer = _collisionAction.collisionLayers[(int)layer];
        data.position = transform.position + genOffset;
        data.radius = radius;
        data.time = 2;
        data.damage = damage;

        // ����
        CreateCollision.instance.CreateCollisionSphere(gameObject, data);
    }
}
