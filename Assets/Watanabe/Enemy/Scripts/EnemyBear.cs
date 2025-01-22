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

    Quaternion rotation;

    private void Update()
    {
        enemyForward = Vector3.forward;
        relativePosition = GetRelativePosition();

        // プレイヤーの方を向く
        rotation = LookAtMe();
        SetRotation(rotation);

        distance = DistanceToPlayer();

        if (!selfAnimator.GetBool("Found"))
        {
            if (distance <= 5.0f)
            {
                SetAnimatorBool("Found", true);
                SetAnimatorBool("Running", true);
            }
        }

        else
        {
            if (distance <= 3.0f)
            {
                SetAnimatorTrigger("Attack");
            }
        }



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
}
