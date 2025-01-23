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
    protected GameObject player;                // プレイヤー
    protected float breakPoint;                 // ブレイク値
    public Vector3 relativePosition;

    public Vector3 enemyForward;

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

    public void SetAnimatorTrigger(string triggerName) { selfAnimator?.SetTrigger(triggerName); }

    public void SetAnimatorBool(string boolName, bool value) { selfAnimator?.SetBool(boolName, value); }

    // アニメーションで更新されたオブジェクトの座標を保存
    public void PositionUpdate() { position = gameObject.transform.position; }

    // プレイヤーとの相対距離を取得
    public Vector3 GetRelativePosition()
    {
        Vector3 playerPos = player.transform.position;
         return player.transform.position - gameObject.transform.position;
    }

    public Quaternion LookAtMe() { return Quaternion.LookRotation(relativePosition, Vector3.up); }
    public void SetRotation(Quaternion rotation)
    {
        rotation.x = 0f;
        rotation.z = 0f;
        gameObject.transform.rotation = rotation; 
    }

    // プレイヤーまでの距離
    public float DistanceToPlayer() { return Vector3.Distance(transform.position, player.transform.position); }

    // ブレイク値を変動させる
    public void ReceiveBreakPoint(float breakSize) { breakPoint -= breakSize; }

    // ステートの情報を更新
    public void UpdataState(GameConst.EnemyState state) { nowState = state; }
}
