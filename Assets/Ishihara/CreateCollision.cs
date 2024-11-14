using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class CreateCollision : MonoBehaviour
{
    // ���̃N���X��static�Ƃ��ăC���X�^���X����
    public static CreateCollision instance;

    // public //////////////////////////////////////////////////////////////////

    /// <summary>�U���̃f�[�^</summary>
    public struct AttackData
    {
        public Vector3 position;
        public float radius;
        public float time;
        public string layer;
        public string tagname;

        public AttackData zero()
        {
            position = Vector3.zero;
            radius = 0f;
            time = 0f;
            layer = string.Empty;
            tagname = string.Empty;

            return new AttackData(Vector3.zero, 0f, 0f, string.Empty, string.Empty);
        }

        public AttackData(Vector3 pos, float radius, float time, string layer, string tag)
        {
            this.position = pos;
            this.radius = radius;
            this.time = time;
            this.layer = layer;
            this.tagname = tag;
        }

        public void CheckNull()
        {
            if (position == null) position = Vector3.zero;
            if (radius == 0f) radius = 0f;
            if (time == 0f) time = 0f;
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
                    Debug.Log("Tag '" + tag + "' has been added.");
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
                        Debug.Log("Layer '" + layer + "' has been added.");
                        return;
                    }
                }
                Debug.LogWarning("Layer limit reached. Could not add layer '" + layer + "'.");
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �����蔻��𐶐�
    /// </summary>
    /// <param name="parent">       �e�̃I�u�W�F�N�g    </param>
    /// <param name="attackData">   �����蔻��̏��    </param>
    public void CreateCollisionSphere(GameObject parent, AttackData attackData)
    {
        // ����̃Q�[���I�u�W�F�N�g��e�̃I�u�W�F�N�g�����ɐ�������
        GameObject attack = Instantiate(
            new GameObject(parent.name + "Attack"),
            attackData.position,
            parent.transform.rotation,
            parent.transform
            );

        // �Q�[���I�u�W�F�N�g�̐ݒ�
        attackData.CheckTagAndLayer();
        attack.tag = attackData.tagname;
        attack.layer = LayerMask.NameToLayer(attackData.layer);

        // �V�����X�t�B�A�R���C�_�[���A�^�b�`
        SphereCollider sphere = attack.AddComponent<SphereCollider>();

        // �����蔻��̐ݒ�ꗗ
        sphere.center = Vector3.zero;
        sphere.radius = attackData.radius;
        sphere.isTrigger = true;

        // �Q�[���I�u�W�F�N�g�Ɏ��Ԑ����X�N���v�g���A�^�b�`���Ď��Ԃ�ݒ�
        attack.AddComponent<LimitTime>().SetPeriod(attackData.time);
    }
}
