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
    protected GameObject player;                // �v���C���[
    protected float breakPoint;                 // �u���C�N�l
    public Vector3 relativePosition;

    public Vector3 enemyForward;

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

    public void SetAnimatorTrigger(string triggerName) { selfAnimator?.SetTrigger(triggerName); }

    public void SetAnimatorBool(string boolName, bool value) { selfAnimator?.SetBool(boolName, value); }

    // �A�j���[�V�����ōX�V���ꂽ�I�u�W�F�N�g�̍��W��ۑ�
    public void PositionUpdate() { position = gameObject.transform.position; }

    // �v���C���[�Ƃ̑��΋������擾
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

    // �v���C���[�܂ł̋���
    public float DistanceToPlayer() { return Vector3.Distance(transform.position, player.transform.position); }

    // �u���C�N�l��ϓ�������
    public void ReceiveBreakPoint(float breakSize) { breakPoint -= breakSize; }

    // �X�e�[�g�̏����X�V
    public void UpdataState(GameConst.EnemyState state) { nowState = state; }
}
