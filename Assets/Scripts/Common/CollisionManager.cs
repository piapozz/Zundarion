using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using static CommonModule;

public class CollisionManager : SystemObject
{
    // このクラスをstaticとしてインスタンスする
    public static CollisionManager instance = null;

    [SerializeField]
    private GameObject _collisionOrigin = null;     // 当たり判定の元オブジェ

    [SerializeField]
    public Transform unuseCollisionRoot = null;     // 未使用のコリジョンの親

    private List<GameObject> _useCollisionList = null;      // 使用中のコリジョンリスト
    private List<GameObject> _unuseCollisionList = null;    // 未使用のコリジョンリスト

    public List<BaseCharacter> parryList = null;        // パリィに使うリスト

    public readonly int COLLISION_MAX = 10;         // コリジョンの最大数

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
    /// 当たり判定を生成
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
    /// コリジョンを使う時にリストから出し入れしてオブジェクトを返す
    /// </summary>
    /// <returns></returns>
    public GameObject UseCollision()
    {
        ReinsertListMember(ref _unuseCollisionList, ref _useCollisionList);
        GameObject useCollision = _useCollisionList[_useCollisionList.Count - 1];
        return useCollision;
    }

    /// <summary>
    /// 使い終わったコリジョンをリストに入れなおす
    /// </summary>
    /// <param name="unuseCollision"></param>
    public void UnuseCollision(GameObject unuseCollision)
    {
        ReinsertListMember(ref _useCollisionList, ref _unuseCollisionList, unuseCollision);
        unuseCollision = _unuseCollisionList[_unuseCollisionList.Count - 1];
        unuseCollision.transform.parent = unuseCollisionRoot;
    }

    /// <summary>
    /// コリジョンを生成するときにデータの設定をする
    /// </summary>
    /// <param name="genObj"></param>
    /// <param name="ID"></param>
    /// <param name="attackData"></param>
    /// <param name="setTransform"></param>
    public void SetCollision(ref GameObject genObj, int ID, CharacterAttackData attackData, Transform setTransform)
    {
        // 判定のデータ設定
        CollisionData collisionData = genObj.GetComponent<CollisionData>();
        collisionData.damage = attackData.damage;
        collisionData.isParry = attackData.isParry;
        collisionData.characterID = ID;
        // 生成時間設定
        collisionData.deleteTime = attackData.generateTime;
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
        genObj.transform.position = genPos;
        genObj.transform.SetParent(setTransform);
    }
}
