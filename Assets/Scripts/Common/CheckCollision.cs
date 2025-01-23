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
    private string playerTag = null;
    private string enemyTag = null;
    private string thisTag = null;

    public List<BaseCharacter> parryList { get; private set; } = new List<BaseCharacter>(3);

    private BaseCharacter _character = null;

    private void Start()
    {
        playerTag = "Player";
        enemyTag = "Enemy";
        thisTag = gameObject.tag;
        _character = GetComponent<BaseCharacter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject hitObj = other.gameObject;
        string hitTagName = hitObj.tag;

        // 判定すべきコリジョンか判定
        CollisionData collisionData = hitObj.GetComponent<CollisionData>();
        if (collisionData == null || !JudgeTag(hitTagName)) return;

        bool isParry = collisionData.isParry;
        int hitID = collisionData.characterID;
        BaseCharacter hitCharacter = CharacterManager.instance.GetCharacter(hitID);
        if (hitCharacter == null) return;
        // パリィでリストに入っていないなら
        if (isParry)
        {
            if (parryList.Exists(x => x == hitCharacter)) return;

            // リストに入れる
            parryList.Add(hitCharacter);
        }
        else
        {
            // ダメージ判定
            hitCharacter.TakeDamage(collisionData.damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // パリィリストから削除
        GameObject hitObj = other.gameObject;
        BaseCharacter character = hitObj.GetComponent<BaseCharacter>();
        if (parryList.Exists(x => x == character))
            parryList.Remove(character);
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
}
