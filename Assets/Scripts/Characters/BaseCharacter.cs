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

        health -= damage;

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
        dir.y = position.y;
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

    //public async UniTask RotateEvent(RotateEventData eventData)
    //{
    //    float frameCount = 0.0f;
    //    Vector3 targetVec;
    //    GameObject player = CharacterManager.instance.playerObject;

    //    while (true)
    //    {
    //        Debug.Log("muiteru");
    //        // ���[�v�𔲂������
    //        if (frameCount >= eventData.frame) break;
    //        else { frameCount += 1; }

    //        targetVec = GetTargetVec(player.transform.position);

    //        // �ړ��x�N�g�����[���łȂ��ꍇ�̂ݏ�����i�߂�
    //        if (targetVec == Vector3.zero) break;

    //        // �ڕW�̊p�x���v�Z
    //        float targetAngle = Mathf.Atan2(targetVec.z, targetVec.x) * Mathf.Rad2Deg;

    //        // ���݂̉�]�p�x����ڕW�̊p�x�֕��
    //        Quaternion targetRotation = Quaternion.AngleAxis(targetAngle - 90, Vector3.down);
    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, eventData.speed * Time.deltaTime);

    //        // 1�t���[���҂�
    //        await UniTask.DelayFrame(1);

    //    }
    //}

    /// <summary>
    /// �O�i������
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    public void Move(Vector3 dir, float speed)
    {
        // �����Ă�������Ɉړ�
        Vector3 movePosition = transform.position + (transform.forward * speed * Time.deltaTime);
        // Vector3 movePosition = transform.position + (dir * speed * Time.deltaTime);

        transform.position = movePosition;
    }

    /// <summary>
    /// �������������ɏ��X�Ɍ�������
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    public void Rotate(Vector3 dir, float speed)
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
    /// �̗͂�n��
    /// </summary>
    /// <returns></returns>
    public float GetCharacterHealth() { return health; }

    public abstract bool IsPlayer();

}
