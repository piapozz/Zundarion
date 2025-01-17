/*
 * @file BaseEnemy.cs
 * @brief �G�̃x�[�X�N���X
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

    protected float breakPoint;                 // �u���C�N�l
    

    public float detectionRange = 10f;
    public float attackRange = 2f;

    // �A�j���[�V�����ōX�V���ꂽ�I�u�W�F�N�g�̍��W��ۑ�
    public void PositionUpdate() { position = gameObject.transform.position; }

    // �v���C���[�܂ł̋���
    public bool IsPlayerInRange() { return Vector3.Distance(transform.position, player.position) <= detectionRange; }

    // �U���͈�
    public bool IsPlayerInAttackRange() { return Vector3.Distance(transform.position, player.position) <= attackRange; }

    // �u���C�N�l��ϓ�������
    public void ReceiveBreakPoint(float breakSize) { breakPoint -= breakSize; }
}
