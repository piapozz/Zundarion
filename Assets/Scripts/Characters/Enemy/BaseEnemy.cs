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
    public BasePlayer player { get; protected set; } = null;    // プレイヤー
    public float breakPoint { get; protected set; } = -1;       // ブレイク値
    public float distance { get; protected set; } = -1;
    public Vector3 targetVec;

    public IEnemyState enemyState = null;

    private void Start()
    {
        selfAnimator = GetComponent<Animator>();
        player = CharacterManager.instance.player;
    }

    public void SetAnimatorTrigger(string triggerName) { selfAnimator?.SetTrigger(triggerName); }

    public void SetAnimatorBool(string boolName, bool value) { selfAnimator?.SetBool(boolName, value); }

    public void PlayerStatus()
    {
        // 目標に向かう向きを取得
        targetVec = GetTargetVec(player.transform.position);

        // 距離を取得
        distance = GetRelativePosition(player.transform).magnitude;
    }

    // ブレイク値を変動させる
    public void ReceiveBreakPoint(float breakSize) { breakPoint -= breakSize; }

    public override bool IsPlayer() { return false; }

    public override void TakeDamage(float damageSize)
    {
        base.TakeDamage(damageSize);
        if (health <= 0)
            selfAnimator.SetBool("Dying", true);
    }
    public void ChangeState(IEnemyState newState)
    {
        if (enemyState == newState || newState == null) return;
        
        if(enemyState != null)enemyState.Exit(this);
        enemyState = newState;
        enemyState.Enter(this);
    }

    public void EnemyAction()
    {
        if (enemyState != null)
        {
            enemyState.Execute(this);
        }
    }

    public Vector3 GetEnemyPosition() { return transform.position; }
}
