using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using static CommonModule;

public class CollisionManager : SystemObject
{
    // ���̃N���X��static�Ƃ��ăC���X�^���X����
    public static CollisionManager instance = null;

    [SerializeField]
    private GameObject _collisionOrigin = null;     // �����蔻��̌��I�u�W�F

    [SerializeField]
    public Transform unuseCollisionRoot = null;     // ���g�p�̃R���W�����̐e

    private List<GameObject> _useCollisionList = null;      // �g�p���̃R���W�������X�g
    private List<GameObject> _unuseCollisionList = null;    // ���g�p�̃R���W�������X�g

    public List<BaseCharacter> parryList = null;        // �p���B�Ɏg�����X�g

    public readonly int COLLISION_MAX = 10;         // �R���W�����̍ő吔

    public override void Initialize()
    {
        instance = this;

        _useCollisionList = new List<GameObject>(COLLISION_MAX);
        _unuseCollisionList = new List<GameObject>(COLLISION_MAX);

        for (int i = 0; i < COLLISION_MAX; i++)
        {
            _unuseCollisionList.Add(Instantiate(_collisionOrigin, unuseCollisionRoot));
        }

        parryList = new List<BaseCharacter>(COLLISION_MAX);
    }

    /// <summary>
    /// �����蔻��𐶐�
    /// </summary>
    /// <param name="attackData"></param>
    /// <param name="setTransform"></param>
    public void CreateCollisionSphere(int ID, CharacterAttackData attackData, Transform setTransform)
    {
        GameObject genObj = UseCollision();
        if (genObj == null) return;

        SetCollision(ref genObj, ID, attackData, setTransform);
    }

    /// <summary>
    /// �R���W�������g�����Ƀ��X�g����o�����ꂵ�ăI�u�W�F�N�g��Ԃ�
    /// </summary>
    /// <returns></returns>
    public GameObject UseCollision()
    {
        ReinsertListMember(ref _unuseCollisionList, ref _useCollisionList);
        GameObject useCollision = _useCollisionList[_useCollisionList.Count - 1];
        return useCollision;
    }

    /// <summary>
    /// �g���I������R���W���������X�g�ɓ���Ȃ���
    /// </summary>
    /// <param name="unuseCollision"></param>
    public void UnuseCollision(GameObject unuseCollision)
    {
        ReinsertListMember(ref _useCollisionList, ref _unuseCollisionList, unuseCollision);
        unuseCollision = _unuseCollisionList[_unuseCollisionList.Count - 1];
        unuseCollision.transform.parent = unuseCollisionRoot;
    }

    /// <summary>
    /// �R���W�����𐶐�����Ƃ��Ƀf�[�^�̐ݒ������
    /// </summary>
    /// <param name="genObj"></param>
    /// <param name="ID"></param>
    /// <param name="attackData"></param>
    /// <param name="setTransform"></param>
    public void SetCollision(ref GameObject genObj, int ID, CharacterAttackData attackData, Transform setTransform)
    {
        // ����̃f�[�^�ݒ�
        CollisionData collisionData = genObj.GetComponent<CollisionData>();
        collisionData.damage = attackData.damage;
        collisionData.isParry = attackData.isParry;
        collisionData.characterID = ID;
        // �������Ԑݒ�
        collisionData.deleteTime = attackData.generateTime;
        // ���a�ݒ�
        genObj.transform.localScale = Vector3.one * attackData.scale;
        // �^�O�ݒ�
        genObj.tag = setTransform.tag;
        // ���W�ݒ�
        Vector3 genPos = setTransform.position;
        float angle = setTransform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float distance = attackData.distance;
        Vector3 offset = new Vector3(Mathf.Sin(angle) * distance, 1, Mathf.Cos(angle) * distance);
        genPos += offset;
        genObj.transform.position = genPos;
        genObj.transform.SetParent(setTransform);
    }
}
