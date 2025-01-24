/*
 * @file EnemyBear.cs
 * @brief Bearを管理するクラス
 * @author sein
 * @date 2025/1/17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBear : BaseEnemy
{
    float distance;

    Quaternion targetRotation;

    private void Update()
    {
        SetRotation(targetRotation);
        // 距離を取得
        distance = GetRelativePosition().magnitude;
        Debug.Log(distance);
    }

    public override void Attack()
    {

    }

    public override void StrongAttack()
    {

    }

    public override void UniqueAttack()
    {

    }

    public override void Restraint()
    {
        // 追跡範囲よりも離れていたら
        if (distance >= 15.0f)
        {
            // 見失う
            SetAnimatorBool("Found", false);
            SetAnimatorBool("Restraint", false);
            return;
        }

        // 弱攻撃か強攻撃をする
        if (distance <= 3.0f)
        {
            int attackType = Random.Range(0, 2);
            if (attackType == 0) SetAnimatorTrigger("Attack");
            if (attackType == 1) SetAnimatorTrigger("StrongAttack");
        }
        // ジャンプ攻撃に遷移
        else if (distance >= 6.0f && distance <= 10.0f)
        {
            SetAnimatorTrigger("UniqueAttack");
        }
        // 追跡に遷移
        else
        {
            SetAnimatorBool("Chasing", true);
        }
    }

    public override void Wandering()
    {
        // 発見
        if (distance <= 7.0f) SetAnimatorBool("Found", true);
    }

    public override void Found()
    {

    }

    public override void Chasing()
    {
        if (distance <= 3.0f) SetAnimatorBool("Chasing", false);

        Transform enemyTransform = transform;
        Transform playerTransform = player.transform;

        // プレイヤーの方向を計算
        Vector3 directionToPlayer = (playerTransform.position - enemyTransform.position).normalized;

        // プレイヤーの方向を向く
        targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
    }

    public override void Dying()
    {

    }
}
