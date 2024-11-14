using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class CreateCollision : MonoBehaviour
{
    // このクラスをstaticとしてインスタンスする
    public static CreateCollision instance;

    // public //////////////////////////////////////////////////////////////////

    /// <summary>攻撃のデータ</summary>
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
            // タグが存在するか確認
            if (!IsTagExists(tag))
            {
                // 存在しない場合は追加
                SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
                SerializedProperty tagsProp = tagManager.FindProperty("tags");

                // タグが空でないか確認し、タグに追加
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
            // レイヤーが存在するか確認
            if (!IsLayerExists(layer))
            {
                // 存在しない場合は追加
                SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
                SerializedProperty layersProp = tagManager.FindProperty("layers");

                // レイヤーの最大数32に達していないか確認し、追加
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
    /// 当たり判定を生成
    /// </summary>
    /// <param name="parent">       親のオブジェクト    </param>
    /// <param name="attackData">   当たり判定の情報    </param>
    public void CreateCollisionSphere(GameObject parent, AttackData attackData)
    {
        // からのゲームオブジェクトを親のオブジェクト直下に生成する
        GameObject attack = Instantiate(
            new GameObject(parent.name + "Attack"),
            attackData.position,
            parent.transform.rotation,
            parent.transform
            );

        // ゲームオブジェクトの設定
        attackData.CheckTagAndLayer();
        attack.tag = attackData.tagname;
        attack.layer = LayerMask.NameToLayer(attackData.layer);

        // 新しくスフィアコライダーをアタッチ
        SphereCollider sphere = attack.AddComponent<SphereCollider>();

        // 当たり判定の設定一覧
        sphere.center = Vector3.zero;
        sphere.radius = attackData.radius;
        sphere.isTrigger = true;

        // ゲームオブジェクトに時間制限スクリプトをアタッチして時間を設定
        attack.AddComponent<LimitTime>().SetPeriod(attackData.time);
    }
}
