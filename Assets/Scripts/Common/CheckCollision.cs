/*
* @file CheckCollision.cs
* @brief キャラクターにアタッチして判定をする
* @author sein
* @date 2025/1/17
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    [SerializeField] CollisionAction collisionAction;

    public bool canParry { get; private set; } = false;
    private string playerTag = null;
    private string enemyTag = null;
    private string thisTag = null;

    public List<GameObject> parryList { get; private set; } = new List<GameObject>(3);

    private void Start()
    {
        playerTag = collisionAction.collisionTags[(int)CollisionAction.CollisionTag.PLAYER];
        enemyTag = collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ENEMY];
        thisTag = transform.parent.gameObject.tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject hitObj = other.gameObject;
        string hitTagName = hitObj.tag;
        string hitLayerName = LayerMask.LayerToName(hitObj.layer);
        // 敵のコリジョンか判定
        if (!JudgeTag(hitTagName)) return;

        // パリィリストに入れる
        if (JudgeParry(hitLayerName) && !parryList.Exists(x => x == hitObj))
        {
            parryList.Add(hitObj);
            return;
        }

        // ダメージ判定
        TakeDamage(hitObj, hitLayerName);
    }

    private void OnTriggerExit(Collider other)
    {
        // パリィリストから削除
        GameObject hitObj = other.gameObject;
        if (parryList.Exists(x => x == hitObj))
            parryList.Remove(hitObj);
    }

    /// <summary>
    /// 敵のタグか判定する
    /// </summary>
    /// <param name="hitTag"></param>
    /// <returns></returns>
    private bool JudgeTag(string hitTag)
    {
        if ((hitTag == enemyTag && thisTag == playerTag) ||
            (hitTag == playerTag && thisTag == enemyTag))
            return true;
        else
            return false;
    }

    private bool JudgeParry(string hitLayerName)
    {
        // 接触したコリジョンが攻撃予兆か判定
        if (hitLayerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.ATTACK_OMEN])
            return true;
        else
            return false;
    }

    private void TakeDamage(GameObject hitObj, string hitLayerName)
    {
        if (hitLayerName != collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.ATTACK_NORMAL])
            return;

        float damage = hitObj.GetComponent<DealDamage>().damage;
        gameObject.BaseCharacter.TakeDamage(damage);
    }
}
