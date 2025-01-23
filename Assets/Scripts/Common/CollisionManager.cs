using System.Collections;
using UnityEditor;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    // このクラスをstaticとしてインスタンスする
    public static CollisionManager instance;

    [SerializeField]
    private GameObject _collisionOrigin;    // 当たり判定の元オブジェ

    [SerializeField]
    private Transform _rootCollision;       // 当たり判定生成する親

    // public //////////////////////////////////////////////////////////////////
    /*
    /// <summary>攻撃のデータ</summary>
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

    // オブジェクトプーリングでやる予定
    /// <summary>
    /// 当たり判定を生成
    /// </summary>
    /// <param name="attackData"></param>
    /// <param name="setTransform"></param>
    public void CreateCollisionSphere(int ID, CharacterAttackData attackData, Transform setTransform)
    {
        GameObject genObj = _collisionOrigin;
        // 判定のデータ設定
        CollisionData collisionData = genObj.GetComponent<CollisionData>();
        collisionData.damage = attackData.damage;
        collisionData.isParry = attackData.isParry;
        collisionData.characterID = ID;
        // 生成時間設定
        LimitTime limitTime = genObj.GetComponent<LimitTime>();
        limitTime.deleteTime = attackData.generateTime;
        // 半径設定
        genObj.transform.localScale = Vector3.one * attackData.scale;
        // タグ設定
        genObj.tag = setTransform.tag;
        // 座標設定
        Vector3 genPos = setTransform.position;
        float angle = setTransform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float distance = attackData.distance;
        Vector3 offset = new Vector3(Mathf.Sin(angle) * distance, 1, Mathf.Cos(angle) * distance);
        genPos += offset;
        // 生成
        Instantiate(
            genObj,
            genPos,
            Quaternion.identity,
            _rootCollision
            );
    }
}
