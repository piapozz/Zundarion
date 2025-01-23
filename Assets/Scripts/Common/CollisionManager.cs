using System.Collections;
using UnityEditor;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    // ���̃N���X��static�Ƃ��ăC���X�^���X����
    public static CollisionManager instance;

    [SerializeField]
    private GameObject _collisionOrigin;    // �����蔻��̌��I�u�W�F

    [SerializeField]
    private Transform _rootCollision;       // �����蔻�萶������e

    // public //////////////////////////////////////////////////////////////////
    /*
    /// <summary>�U���̃f�[�^</summary>
    public struct AttackData
    {
        public Vector3 position;
        public float radius;
        public float time;
        public float damage;
        public string layer;
        public string tagname;

        public AttackData zero()
        {
            position = Vector3.zero;
            radius = 0f;
            time = 0f;
            layer = string.Empty;
            tagname = string.Empty;

            return new AttackData(Vector3.zero, 0f, 0f, 0f, string.Empty, string.Empty);
        }

        public AttackData(Vector3 pos, float radius, float time, float damage, string layer, string tag)
        {
            this.position = pos;
            this.radius = radius;
            this.time = time;
            this.damage = damage;
            this.layer = layer;
            this.tagname = tag;
        }

        public void CheckNull()
        {
            if (position == null) position = Vector3.zero;
            if (radius == 0f) radius = 0f;
            if (time == 0f) time = 0f;
            if (damage == 0f) damage = 0f;
            if (layer == string.Empty) layer = "Empty";
            if (tagname == string.Empty) tagname = "Empty";
        }

        public void CheckTagAndLayer()
        {
            AddTagIfNotExists(tagname);
            AddLayerIfNotExists(layer);
        }

        public void AddTagIfNotExists(string tag)
        {
            // �^�O�����݂��邩�m�F
            if (!IsTagExists(tag))
            {
                // ���݂��Ȃ��ꍇ�͒ǉ�
                SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
                SerializedProperty tagsProp = tagManager.FindProperty("tags");

                // �^�O����łȂ����m�F���A�^�O�ɒǉ�
                if (tagsProp.arraySize == 0 || !IsTagExists(tag))
                {
                    tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
                    tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1).stringValue = tag;
                    tagManager.ApplyModifiedProperties();
                }
            }
        }

        public void AddLayerIfNotExists(string layer)
        {
            // ���C���[�����݂��邩�m�F
            if (!IsLayerExists(layer))
            {
                // ���݂��Ȃ��ꍇ�͒ǉ�
                SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
                SerializedProperty layersProp = tagManager.FindProperty("layers");

                // ���C���[�̍ő吔32�ɒB���Ă��Ȃ����m�F���A�ǉ�
                for (int i = 8; i < layersProp.arraySize; i++)
                {
                    SerializedProperty layerSP = layersProp.GetArrayElementAtIndex(i);
                    if (string.IsNullOrEmpty(layerSP.stringValue))
                    {
                        layerSP.stringValue = layer;
                        tagManager.ApplyModifiedProperties();
                        return;
                    }
                }
            }
        }

        private bool IsTagExists(string tag)
        {
            foreach (string existingTag in UnityEditorInternal.InternalEditorUtility.tags)
            {
                if (existingTag == tag)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsLayerExists(string layer)
        {
            foreach (string existingLayer in UnityEditorInternal.InternalEditorUtility.layers)
            {
                if (existingLayer == layer)
                {
                    return true;
                }
            }
            return false;
        }
    }
    */

    private void Awake()
    {
        instance = this;
    }

    // �I�u�W�F�N�g�v�[�����O�ł��\��
    /// <summary>
    /// �����蔻��𐶐�
    /// </summary>
    /// <param name="attackData"></param>
    /// <param name="setTransform"></param>
    public void CreateCollisionSphere(int ID, CharacterAttackData attackData, Transform setTransform)
    {
        GameObject genObj = _collisionOrigin;
        // ����̃f�[�^�ݒ�
        CollisionData collisionData = genObj.GetComponent<CollisionData>();
        collisionData.damage = attackData.damage;
        collisionData.isParry = attackData.isParry;
        collisionData.characterID = ID;
        // �������Ԑݒ�
        LimitTime limitTime = genObj.GetComponent<LimitTime>();
        limitTime.deleteTime = attackData.generateTime;
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
        // ����
        Instantiate(
            genObj,
            genPos,
            Quaternion.identity,
            _rootCollision
            );
    }
}
