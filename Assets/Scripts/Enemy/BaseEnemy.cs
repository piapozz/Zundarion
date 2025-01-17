/*
 * @file BaseEnemy.cs
 * @brief 敵のベースクラス
 * @author sein
 * @date 2025/1/17
 */

using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class BaseEnemy : BaseCharacter
{
    protected Transform player;

    protected float breakPoint;                 // ブレイク値
    

    public float detectionRange = 10f;
    public float attackRange = 2f;

    // アニメーションで更新されたオブジェクトの座標を保存
    public void PositionUpdate() { position = gameObject.transform.position; }

    // プレイヤーまでの距離
    public bool IsPlayerInRange() { return Vector3.Distance(transform.position, player.position) <= detectionRange; }

    // 攻撃範囲
    public bool IsPlayerInAttackRange() { return Vector3.Distance(transform.position, player.position) <= attackRange; }

    // ブレイク値を変動させる
    public void ReceiveBreakPoint(float breakSize) { breakPoint -= breakSize; }
}
