using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 敵クラスの親
// 基礎ステータスと使用されるAIはScriptableObjectでいじれるように作る
// UpdateEnemy関数を子クラスで編集してAIを作る

public abstract class EnemyBase : MonoBehaviour
{
    public EnemyStatus enemyStatus;                 // ScriptableObjectの情報

    private Animator animator;                      // 使用するanimator

    // 敵のステータス
    public struct Status
    {
        public int m_enemyNum;

        public float m_health;
        public float m_speed;
        public float m_strength;
        public float m_break;
        public Vector3 m_position;
        public Vector3 m_positionNext;
    }

    public Status status;

    public enum Patterns
    {
        NEUTRAL = 0,
        ATTACK,

        MAX
    }

    // 敵の行動
    protected abstract void UpdateEnemy();

    // 初期設定
    private void Awake()
    {
        InputStatus();
        animator = gameObject.GetComponent<Animator>();
    }

    // 敵の処理
    private void Update()
    {
        // AIの挙動
        UpdateEnemy();
    }

    // ステータスの初期化
    private void InputStatus()
    {
        // ScriptableObjectの情報を読み込む
        status.m_health = enemyStatus.health;
        status.m_strength = enemyStatus.strength;
        status.m_speed = enemyStatus.speed;
    }

    // アニメーションで更新されたオブジェクトの座標を保存
    private void PositionUpdate()
    {
        status.m_position = gameObject.transform.position;
    }

    private float GetDamage(float damageSize) { return damageSize / 2.0f; }
}
