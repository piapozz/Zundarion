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
    private TagData _collisionAction = null;

    // �L�����N�^�[�̃X�e�[�^�X
    protected float healthMax;         // �ő�̗�
    protected float health;            // ���݂̗̑�
    protected float strength;          // �U����
    protected float defence;           // �h���
    protected float speed;             // ����

    protected Vector3 position;        // ���W
    protected float multiplier;        // �{��

    /// <summary>�A�j���[�^�[�R���|�[�l���g</summary>
    public Animator selfAnimator = null;

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
        selfAnimator = GetComponent<Animator>();
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

    /// <summary>
    /// �����蔻��𐶐�����
    /// </summary>
    /// <param name="attackData"></param>
    public void CreateCollisionEvent(CharacterAttackData attackData)
    {
        // ����
        CreateCollision.instance.CreateCollisionSphere(attackData, transform);
    }

    /// <summary>
    /// �ڕW�̕����ɐU�����
    /// </summary>
    /// <param name="target"></param>
    public void TurnAround(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        dir.y = position.y;
        Quaternion dirRot = Quaternion.LookRotation(dir);
        transform.rotation = dirRot;
    }
}
