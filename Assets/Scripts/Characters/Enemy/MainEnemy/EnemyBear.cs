/*
 * @file EnemyBear.cs
 * @brief Bear���Ǘ�����N���X
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
        // �ڕW�Ɍ������������擾
        targetVec = GetTargetVec(player.transform.position);

        // �������擾
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
        // �ǐՔ͈͂�������Ă�����
        if (distance >= 15.0f)
        {
            // ������
            SetAnimatorBool("Found", false);
            SetAnimatorBool("Restraint", false);
            return;
        }

        // ��U�������U��������
        if (distance <= 3.0f)
        {
            int attackType = Random.Range(0, 2);

            if (attackType == 0) SetAnimatorTrigger("Attack");
            if (attackType == 1) SetAnimatorTrigger("StrongAttack");
        }
        // �W�����v�U���ɑJ��
        else if (distance >= 6.0f && distance <= 10.0f)
        {
            SetAnimatorTrigger("UniqueAttack");
        }
        // �ǐՂɑJ��
        else
        {
            SetAnimatorBool("Chasing", true);
        }
    }

    public override void Wandering()
    {
        // ����
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

        // �ǐՔ͈͂�������Ă�����
        if (distance >= 15.0f)
        {
            // ������
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
