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

// �e�N���X��public���C���X�y�N�^�[��ɕ\��
#if UNITY_EDITOR
[CustomEditor(typeof(BaseCharacter))]
#endif

public abstract class BaseCharacter : MonoBehaviour
{
    [SerializeField] 
    private CharacterData charaData = null;       // �L�����N�^�[�̃f�[�^

    public int ID { get; private set; } = -1;

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
    public void Initialize(int setID)
    {
        ID = setID;
        healthMax = charaData.health;
        health = healthMax;
        strength = charaData.strength;
        defence = charaData.defence;
        speed = charaData.speed;
        selfAnimator = GetComponent<Animator>();
    }

    public void SetTransform(Transform setTransform)
    {
        transform.position = setTransform.position;
        transform.rotation = setTransform.rotation;
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
        CollisionManager.instance.CreateCollisionSphere(ID, attackData, transform);
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
