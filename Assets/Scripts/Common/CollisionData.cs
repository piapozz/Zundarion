/*
* @file CollisionData.cs
* @brief 当たり判定にアタッチする当たり判定のデータ
* @date 2025/1/20
*/

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static CommonModule;
using static GameConst;

public class CollisionData : MonoBehaviour
{
    public int characterID = -1;
    public bool isParry = false;
    public bool isAvoid = false;
    public float damage = -1;
    public int deleteFrame = -1;

    private string thisTag = null;

    /// <summary>
    /// 制限時間を設定し、時間経過後にゲームオブジェクトを破壊します。
    /// </summary>
    /// <param name="time">制限時間（秒）</param>
    public void OnEnable()
    {
        thisTag = transform.tag;
        UniTask task = WaitAction(deleteFrame, LimitOver);
    }

    // 親オブジェクトのdestroyでListから消せてない？
    private void OnDisable()
    {
        RemoveList();
    }

    /// <summary>
    /// アタッチされているオブジェクトを非アクティブにする
    /// </summary>
    private void LimitOver()
    {
        RemoveList();
        CollisionManager.instance.UnuseCollision(gameObject);
    }

    private readonly string PLAYER_TAG = "Player";
    private readonly string ENEMY_TAG = "Enemy";

    private void OnTriggerEnter(Collider other)
    {
        // タグでチェック判定
        GameObject hitObj = other.gameObject;
        string hitTagName = hitObj.tag;
        if (!JudgeHittable(hitTagName)) return;

        // BaseCharacterを取得
        BaseCharacter hitCharacter = hitObj.GetComponent<BaseCharacter>();
        if (hitCharacter == null) return;

        BaseCharacter sourceCharacter = CharacterManager.instance.GetCharacter(characterID);
        if (sourceCharacter == null) return;
        if (isParry)
        {
            // リストに入れる
            CharacterManager.instance.player.AddParryList(this);
        }
        else if (isAvoid)
        {
            // リストに入れる
            CharacterManager.instance.player.AddAvoidList(this);
        }
        else
        {
            // ダメージ判定
            hitCharacter.TakeDamage(damage, sourceCharacter.strength);
            sourceCharacter.SetAnimationSpeed(HIT_STOP_SPEED, HIT_STOP_FRAME);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isParry && !isAvoid) return;
        // タグでチェック判定
        GameObject hitObj = other.gameObject;
        string hitTagName = hitObj.tag;
        if (!JudgeHittable(hitTagName)) return;

        RemoveList();
    }

    /// <summary>
    /// 敵のタグか判定する
    /// </summary>
    /// <param name="hitTag"></param>
    /// <returns></returns>
    private bool JudgeHittable(string hitTag)
    {
        if ((hitTag == ENEMY_TAG && thisTag == PLAYER_TAG) ||
            (hitTag == PLAYER_TAG && thisTag == ENEMY_TAG))
            return true;
        else
            return false;
    }

    private void RemoveList()
    {
        if (isParry)
        {
            CharacterManager.instance.player.RemoveParryList(this);
        }
        else if (isAvoid)
        {
            CharacterManager.instance.player.RemoveAvoidList(this);
        }
    }
}
