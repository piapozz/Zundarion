/*
 * @file BaseEnemy.cs
 * @brief �G�̃x�[�X�N���X
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
    protected GameObject player;               // �v���C���[
    protected float breakPoint;                 // �u���C�N�l
    protected float distance;
    public Vector3 targetVec;

    public GameConst.EnemyState nowState;                // ���݂̃X�e�[�g���Ǘ�

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

    // �v���C���[�Ƃ̑��΋������擾
    public Vector3 GetRelativePosition()
    {
        Vector3 playerPos = player.transform.position;
         return player.transform.position - gameObject.transform.position;
    }

    // �u���C�N�l��ϓ�������
    public void ReceiveBreakPoint(float breakSize) { breakPoint -= breakSize; }
}
