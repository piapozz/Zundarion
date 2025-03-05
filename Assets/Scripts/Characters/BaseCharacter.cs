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

using static CommonModule;
using static GameConst;

// �e�N���X��public���C���X�y�N�^�[��ɕ\��
#if UNITY_EDITOR
[CustomEditor(typeof(BaseCharacter))]
#endif

public abstract class BaseCharacter : MonoBehaviour
{
    [SerializeField] 
    private CharacterData charaData = null;       // �L�����N�^�[�̃f�[�^

    [SerializeField]
    protected int ID = -1;

    /// <summary>�A�j���[�V�����̃p�����[�^�[���</summary>
    [SerializeField]
    protected AnimationData _selfAnimationData;

    /// <summary>���S�t���O</summary>
    protected bool isDead = false;

    /// <summary>���G���ǂ���</summary>
    protected bool isInvincible = false;

    /// <summary>�^�[�Q�b�g�̓G</summary>
    protected BaseCharacter targetEnemy = null;

    /// <summary>�_���[�W�I�u�U�[�o�[</summary>
    [SerializeField]
    private DamageObserver _damageObserver = null;

    [SerializeField]
    private GameObject[] _childObjectArray = null;

    [SerializeField]
    private Transform _effectAnchor = null;

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
    public virtual void Initialize(int setID)
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

    public void SetDamageObserver(DamageObserver observer)
    {
        _damageObserver = observer;
    }

    /// <summary>
    /// �_���[�W���󂯂鏈��
    /// </summary>
    /// <param name="damageRatio"></param>
    public virtual void TakeDamage(float damageRatio, float sourceStrength)
    {
        // �_���[�W����
        int damage = GetDamage(sourceStrength, damageRatio);
        health = Mathf.Max(0, (health - damage));

        _damageObserver.OnDamage(_effectAnchor, damage);

        // �J������h�炷
        CameraManager.instance.SetShake(CAMERA_SHAKE_TIME, CAMERA_SHAKE_GAIN);
    }

    /// <summary>
    /// �v�Z��̃_���[�W���擾����
    /// </summary>
    /// <param name="sourceStrength"></param>
    /// <param name="damageRatio"></param>
    /// <returns></returns>
    protected int GetDamage(float sourceStrength, float damageRatio)
    {
        float defenceRatio = 100 / (defence + 100);
        float damage = damageRatio * sourceStrength;
        return (int)(damage * defenceRatio * multiplier);
    }

    /// <summary>
    /// �Ђ��
    /// </summary>
    public virtual void SetImpact()
    {

    }

    /// <summary>
    /// �A�j���[�V�����̍Đ����Ԃ���莞�ԕς���
    /// </summary>
    public void SetAnimationSpeed(float setSpeed, int frame)
    {
        selfAnimator.SetFloat("Speed", setSpeed);
        UniTask task = WaitAction(frame, () => selfAnimator.SetFloat("Speed", 1));
    }

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
        float length = eventData.length;
        // �ڋ߂���U���Ȃ�
        if (eventData.isApproach)
            length = GetMoveLength(eventData.minLength, eventData.maxLength);

        // direction�����[�J�����W�n�ɕϊ�
        Vector3 direction = transform.TransformDirection(eventData.dir.normalized);

        int frameCount = 0;
        while (moveFrame > frameCount)
        {
            // �w�肳�ꂽ�����Ɉړ�
            transform.position += direction * length / moveFrame;
            //Vector3 nextPos = transform.position + direction * speed / moveFrame;
            //_rigidbody.MovePosition(nextPos);

            frameCount++;
            // 1�t���[���҂�
            await UniTask.DelayFrame(1);
        }
    }

    /// <summary>
    /// �ړ��������擾
    /// </summary>
    /// <param name="minLength"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    private float GetMoveLength(float minLength, float maxLength)
    {
        Vector3 targetPosition = targetEnemy.transform.position;
        Vector3 selfPosition = transform.position;
        float distance = Vector3.Distance(targetPosition, selfPosition);

        return Mathf.Clamp((distance - minLength), 0, maxLength);
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
        EffectManager.instance.GenerateEffect(data, _effectAnchor);
    }

    /// <summary>
    /// ��莞�Ԗ��G�ɂ���C�x���g
    /// </summary>
    /// <param name="frame"></param>
    public void InvincibleEvent(int frame)
    {
        SetInvincible(true);
        UniTask task = WaitAction(frame, SetInvincible, false);
    }

    /// <summary>
    /// ���G�̉ېݒ�
    /// </summary>
    /// <param name="setInvincible"></param>
    public void SetInvincible(bool setInvincible)
    {
        isInvincible = setInvincible;
    }

    /// <summary>
    /// ��莞�ԕs���ɂ���C�x���g
    /// </summary>
    /// <param name="frame"></param>
    /// <returns></returns>
    public void InvisibleEvent(int frame)
    {
        InvisibleAll(false);
        UniTask task = WaitAction(frame, InvisibleAll, true);
    }

    /// <summary>
    /// �q�I�u�W�F�N�g�̃A�N�e�B�u��؂�ւ���
    /// </summary>
    /// <param name="visible"></param>
    private void InvisibleAll(bool visible)
    {
        for (int i = 0, max = _childObjectArray.Length; i < max; i++)
        {
            _childObjectArray[i].gameObject.SetActive(visible);
        }
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
        //_rigidbody.MovePosition(movePosition);
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
    public virtual void Rotate(Vector3 dir, float speed = CHARACTER_ROTATE_SPEED)
    {
        // �ړ��x�N�g�����[���łȂ��ꍇ�̂ݏ�����i�߂�
        if (dir == Vector3.zero) return;

        // �ڕW�̊p�x���v�Z
        float targetAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(targetAngle - 90, Vector3.down);
        transform.rotation = targetRotation;
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
