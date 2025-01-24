/*
* @file CollisionData.cs
* @brief 当たり判定にアタッチする当たり判定のデータ
* @date 2025/1/20
*/

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class CollisionData : MonoBehaviour
{
    public int characterID = -1;
    public bool isParry = false;
    public float damage = -1;
    public float deleteTime = 0.1f;

    /// <summary>
    /// 制限時間を設定し、時間経過後にゲームオブジェクトを破壊します。
    /// </summary>
    /// <param name="time">制限時間（秒）</param>
    public void OnEnable()
    {
        UniTask task = WaitAction(deleteTime, LimitOver);
    }

    /// <summary>
    /// アタッチされているオブジェクトを非アクティブにする
    /// </summary>
    private void LimitOver()
    {
        CollisionManager.instance.UnuseCollision(gameObject);
        BaseCharacter parryTarget = CharacterManager.instance.GetCharacter(characterID);
        CollisionManager.instance.parryList.Remove(parryTarget);
    }
}
