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
    float distance;

    Quaternion targetRotation;

    private void Update()
    {
        Move();
        SetRotation(targetRotation);
        // �������擾
        distance = GetRelativePosition().magnitude;
        // Debug.Log(distance);
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

            targetRotation = GetPlayerDirection();

            if (attackType == 0) SetAnimatorTrigger("Attack");
            if (attackType == 1) SetAnimatorTrigger("StrongAttack");
        }
        // �W�����v�U���ɑJ��
        else if (distance >= 6.0f && distance <= 10.0f)
        {
            targetRotation = GetPlayerDirection();
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

    }

    public override void Chasing()
    {
        if (distance <= 3.0f) SetAnimatorBool("Chasing", false);

        // �ǐՔ͈͂�������Ă�����
        if (distance >= 15.0f)
        {
            // ������
            SetAnimatorBool("Found", false);
            SetAnimatorBool("Restraint", false);
            return;
        }
        targetRotation = GetPlayerDirection();

    }

    public override void Dying()
    {

    }
}
