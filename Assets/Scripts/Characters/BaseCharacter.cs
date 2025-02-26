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

    /// <summary>�_���[�W�I�u�U�[�o�[</summary>
    [SerializeField]
    private DamageObserver _damageObserver = null;

    // �L�����N�^�[�̃X�e�[�^�X
    public float healthMax { get; protected set; } = -1;    // �ő�̗�
    public float health { get; protected set; } = -1;       // ���݂̗̑�
    public float strength { get; protected set; } = -1;      // �U����
    public float defence { get; protected set; } = -1;       // �h���
    public float speed { get; protected set; } = -1;         // ����

    public float multiplier { get; protected set; } = 1;     // �{��

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

    public void SetDamageObserver(DamageObserver observer)
    {
        _damageObserver = observer;
    }

    /// <summary>
    /// �_���[�W���󂯂鏈��
    /// </summary>
    /// <param name="damageSize"></param>
    public virtual void TakeDamage(float damageSize)
    {
        int damage = (int)(damageSize * multiplier);

        health -= damage;

        _damageObserver.OnDamage(transform.position, damage);

        if (health <= 0)
            CharacterManager.instance.RemoveCharacterList(_ID);
    }

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
        dir.y = transform.position.y;
        Quaternion dirRot = Quaternion.LookRotation(dir);
        transform.rotation = dirRot;
    }

    /// <summary>
    /// �A�N�V�����C�x���g���g���čׂ����ړ����s��
    /// </summary>
    /// <param name="eventData">�ړ���񂪓�����ScriptObject</param>
    /// <returns></returns>
    public async UniTask MoveEvent(MoveEventData eventData)
    {
        int moveFrame = eventData.frame;
        float speed = eventData.speed;

        // direction�����[�J�����W�n�ɕϊ�
        Vector3 direction = transform.TransformDirection(eventData.dir.normalized);

        int frameCount = 0;
        while (moveFrame > frameCount)
        {
            // �w�肳�ꂽ�����Ɉړ�
            transform.position += direction * speed / moveFrame;

            frameCount++;
            // 1�t���[���҂�
            await UniTask.DelayFrame(1);
        }
    }

    /// <summary>
    /// ���S�C�x���g
    /// </summary>
    public virtual void DeadEvent()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// �G�t�F�N�g���o���C�x���g
    /// </summary>
    /// <param name="ID"></param>
    public void EffectEvent(EffectGenerateData data)
    {
        EffectManager.instance.GenerateEffect(data, transform);
    }

    /// <summary>
    /// �O�i������
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    public void Move(float speed)
    {
        // �����Ă�������Ɉړ�
        Vector3 movePosition = transform.position + (transform.forward * speed * Time.deltaTime);
        transform.position = movePosition;
    }

    /// <summary>
    /// �w�肵�������Ɉړ�����
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="dir"></param>
    public void Move(float speed, Vector3 dir)
    {
        Vector3 localDir = transform.TransformDirection(dir);
        Vector3 movePosition = transform.position + (localDir * speed * Time.deltaTime);
        transform.position = movePosition;
    }

    /// <summary>
    /// �������������ɏ��X�Ɍ�������
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    public void Rotate(Vector3 dir, float speed = GameConst.CHARACTER_ROTATE_SPEED)
    {
        // �ړ��x�N�g�����[���łȂ��ꍇ�̂ݏ�����i�߂�
        if (dir == Vector3.zero) return;

        // �ڕW�̊p�x���v�Z
        float targetAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        // ���݂̉�]�p�x����ڕW�̊p�x�֕��
        Quaternion targetRotation = Quaternion.AngleAxis(targetAngle - 90, Vector3.down);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime);
    }

    /// <summary>
    /// �ڕW�ւ̕������擾
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public Vector3 GetTargetVec(Vector3 targetPos) { return (targetPos - transform.position).normalized; }

    /// <summary>
    /// �ΏۂƂ̑��΋������擾
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public Vector3 GetRelativePosition(Transform target) { return target.position - gameObject.transform.position; }

    /// <summary>
    /// �̗͂�n��
    /// </summary>
    /// <returns></returns>
    public float GetCharacterHealth() { return health; }

    public abstract bool IsPlayer();

}
