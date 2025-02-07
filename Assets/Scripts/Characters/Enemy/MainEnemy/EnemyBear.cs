/*
 * @file EnemyBear.cs
 * @brief Bear‚ğŠÇ—‚·‚éƒNƒ‰ƒX
 * @author sein
 * @date 2025/1/17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBear : BaseEnemy
{
    private void Update()
    {
        // –Ú•W‚ÉŒü‚©‚¤Œü‚«‚ğæ“¾
        targetVec = GetTargetVec(player.transform.position);

        // ‹——£‚ğæ“¾
        distance = GetRelativePosition().magnitude;
    }

    #region Action
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
        // ’ÇÕ”ÍˆÍ‚æ‚è‚à—£‚ê‚Ä‚¢‚½‚ç
        if (distance >= 15.0f)
        {
            // Œ©¸‚¤
            SetAnimatorBool("Found", false);
            SetAnimatorBool("Restraint", false);
            return;
        }

        // ãUŒ‚‚©‹­UŒ‚‚ğ‚·‚é
        if (distance <= 3.0f)
        {
            int attackType = Random.Range(0, 2);

            if (attackType == 0) SetAnimatorTrigger("Attack");
            if (attackType == 1) SetAnimatorTrigger("StrongAttack");
        }
        // ƒWƒƒƒ“ƒvUŒ‚‚É‘JˆÚ
        else if (distance >= 6.0f && distance <= 10.0f)
        {
            SetAnimatorTrigger("UniqueAttack");
        }
        // ’ÇÕ‚É‘JˆÚ
        else
        {
            SetAnimatorBool("Chasing", true);
        }
    }

    public override void Wandering()
    {
        // ”­Œ©
        if (distance <= 7.0f) SetAnimatorBool("Found", true);
    }

    public override void Found()
    {
        Rotate(targetVec, 200.0f);
    }

    public override void Chasing()
    {
        if (distance <= 2.0f)
        {
            SetAnimatorBool("Chasing", false);
            SetAnimatorTrigger("Attack");
        }

        Move(speed);
        Rotate(targetVec);

        // ’ÇÕ”ÍˆÍ‚æ‚è‚à—£‚ê‚Ä‚¢‚½‚ç
        if (distance >= 15.0f)
        {
            // Œ©¸‚¤
            SetAnimatorBool("Chasing", false);
            SetAnimatorBool("Found", false);
            SetAnimatorBool("Restraint", false);
            return;
        }

    }

    public override void Dying()
    {

    }

    #endregion
}
