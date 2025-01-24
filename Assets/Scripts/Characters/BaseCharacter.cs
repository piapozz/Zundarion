/*
 * @file BaseCharacter.cs
 * @brief �L�����N�^�[���ʂ̏��⏈�����܂Ƃ߂��x�[�X�N���X
 * @author sein
 * @date 2025/1/17
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

// �e�N���X��public���C���X�y�N�^�[��ɕ\��
#if UNITY_EDITOR
[CustomEditor(typeof(BaseCharacter))]
#endif

public abstract class BaseCharacter : MonoBehaviour
{
    [SerializeField] 
    private CharacterData charaData = null;       // �L�����N�^�[�̃f�[�^

    [SerializeField]
    private int _ID = -1;

    // �L�����N�^�[�̃X�e�[�^�X
    [SerializeField] protected float healthMax = -1;         // �ő�̗�
    [SerializeField] protected float health = -1;            // ���݂̗̑�
    protected float strength = -1;          // �U����
    protected float defence = -1;           // �h���
    protected float speed = -1;             // ����

    protected Vector3 position;        // ���W
    protected float multiplier;        // �{��

    /// <summary>�A�j���[�^�[�R���|�[�l���g</summary>
    public Animator selfAnimator = null;

    /// <summary>
    /// ScriptableObject���g���ď�����
    /// </summary>
    public void Initialize(int setID)
    {
        _ID = setID;
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
        CollisionManager.instance.CreateCollisionSphere(_ID, attackData, transform);
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

    /// <summary>
    /// �A�N�V�����C�x���g���g���čׂ����ړ����s��
    /// </summary>
    /// <param name="dir">�ړ�������������</param>
    /// <param name="frame">�ǂꂭ�炢�̃t���[���ړ������邩</param>
    /// <param name="speed">�ړ������鑬��</param>
    /// <returns></returns>
    public async UniTask MoveFine(Vector3 dir, float frame, float speed)
    {
        float frameCount = 0.0f;
        Vector3 movePostion = transform.position;

        dir = dir.normalized;

        while (true)
        {
            // ���[�v�𔲂��鏈��
            if (frameCount >= frame) break;
            else { frameCount += Time.deltaTime; }

            // �w�肳�ꂽ�����Ɉړ�
            movePostion += dir * speed * Time.deltaTime;
            transform.position = movePostion;

            // 1�t���[���҂�
            await UniTask.DelayFrame(1);
        }
    }

    /// <summary>
    /// �O�i������
    /// </summary>
    public void Move()
    {
        // �����Ă�������Ɉړ�
        Vector3 movePosition = transform.position + (transform.forward * speed * Time.deltaTime);

        transform.position = movePosition;
    }

    /// <summary>
    /// �p�x�ɉ����Č�����ύX
    /// </summary>
    /// <param name="moveVec">�ړ�����</param>
    /// <param name="_transform">��ƂȂ����</param>
    public float Rotate(Vector2 moveVec, Transform _transform)
    {
        // �G�̑O���ƉE��������Ɉړ��x�N�g�����v�Z
        Vector3 forward = _transform.forward;
        Vector3 right = _transform.right;

        // �G�̌����Ɋ�Â����ړ��x�N�g�����v�Z
        Vector3 adjustedMove = (right * moveVec.x + forward * moveVec.y).normalized;

        // �������v�Z���čX�V
        float angle = Mathf.Atan2(adjustedMove.z, adjustedMove.x) * Mathf.Rad2Deg;

        return angle;
    }

}
