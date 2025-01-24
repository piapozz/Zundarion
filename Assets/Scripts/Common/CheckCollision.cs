/*
* @file CheckCollision.cs
* @brief キャラクターにアタッチして判定をする
* @author sein
* @date 2025/1/17
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CheckCollision : MonoBehaviour
{
    private readonly string PLAYER_TAG = "Player";
    private readonly string ENEMY_TAG = "Enemy";
    private string _thisTag = null;

    private void Start()
    {
        _thisTag = gameObject.tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        // タグでチェック判定
        GameObject hitObj = other.gameObject;
        string hitTagName = hitObj.tag;
        if (!JudgeTag(hitTagName)) return;

        // CollisionDataを取得
        CollisionData collisionData = hitObj.GetComponent<CollisionData>();
        if (collisionData == null) return;

        // IDからBaseCharacterを取得
        int hitID = collisionData.characterID;
        BaseCharacter hitCharacter = CharacterManager.instance.GetCharacter(hitID);
        if (hitCharacter == null) return;

        bool isParry = collisionData.isParry;
        if (isParry)
            // リストに入れる
            CollisionManager.instance.parryList.Add(hitCharacter);
        else
            // ダメージ判定
            hitCharacter.TakeDamage(collisionData.damage);
    }

    private void OnTriggerExit(Collider other)
    {
        // タグでチェック判定
        GameObject hitObj = other.gameObject;
        string hitTagName = hitObj.tag;
        if (!JudgeTag(hitTagName)) return;

        // CollisionDataを取得
        CollisionData collisionData = hitObj.GetComponent<CollisionData>();
        if (collisionData == null) return;

        // IDからBaseCharacterを取得
        int hitID = collisionData.characterID;
        BaseCharacter hitCharacter = CharacterManager.instance.GetCharacter(hitID);
        if (hitCharacter == null) return;

        bool isParry = collisionData.isParry;
        if (isParry)
            // リストから外す
            CollisionManager.instance.parryList.Remove(hitCharacter);
    }



    /// <summary>
    /// 敵のタグか判定する
    /// </summary>
    /// <param name="hitTag"></param>
    /// <returns></returns>
    private bool JudgeTag(string hitTag)
    {
        if ((hitTag == ENEMY_TAG && _thisTag == PLAYER_TAG) ||
            (hitTag == PLAYER_TAG && _thisTag == ENEMY_TAG))
            return true;
        else
            return false;
    }
}
