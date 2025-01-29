/*
 * @file BaseEnemy.cs
 * @brief 敵のベースクラス
 * @author sein
 * @date 2025/1/17
 */

using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BaseEnemy : BaseCharacter
{
    protected GameObject player;               // プレイヤー
    protected float breakPoint;                 // ブレイク値
    protected float distance;
    public Vector3 targetVec;

    public GameConst.EnemyState nowState;                // 現在のステートを管理

    private void Start()
    {
        nowState = GameConst.EnemyState.INVALID;

        selfAnimator = GetComponent<Animator>();

        player = CharacterManager.instance.playerObject;
    }

    public virtual void Attack() { }
    public virtual void StrongAttack() { }
    public virtual void UniqueAttack() { }
    public virtual void Restraint() { }
    public virtual void Wandering() { }
    public virtual void Found() { }
    public virtual void Chasing() { }
    public virtual void Dying() { }

    public void SetAnimatorTrigger(string triggerName) { selfAnimator?.SetTrigger(triggerName); }

    public void SetAnimatorBool(string boolName, bool value) { selfAnimator?.SetBool(boolName, value); }

    // プレイヤーとの相対距離を取得
    public Vector3 GetRelativePosition()
    {
        Vector3 playerPos = player.transform.position;
         return player.transform.position - gameObject.transform.position;
    }

    // ブレイク値を変動させる
    public void ReceiveBreakPoint(float breakSize) { breakPoint -= breakSize; }
}
