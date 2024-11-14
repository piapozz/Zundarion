using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵クラスの親
// 基礎ステータスと使用されるAIはScriptableObjectでいじれるように作る
// UpdateEnemy関数を子クラスで編集してAIを作る

public abstract class EnemyBase : MonoBehaviour
{
    public InitialStatus initialStatus;             // ScriptableObjectの情報

    // 敵のステータス
    public struct EnemyStatus
    {
        public int m_enemyNum;              // 敵の番号

        public float m_health;              // ヘルス
        public float m_speed;               // スピード
        public float m_power;               // パワー 
        public float m_break;               // ブレイク値
        public float m_distance;            // 敵の視認範囲
        public float m_vision;              // 敵の視野
        public Vector3 m_position;          // 敵の座標
        public Vector3 m_positionNext;      // 敵の移動後予定の座標

        public IEnemyState m_state;         // 敵の状態
        public Animator m_animator;           // キャラクターに使われているAnimator
    }

    protected EnemyStatus status;

    // 敵の行動
    protected abstract void UpdateEnemy();

    // 状況に応じてステータス変更処理
    protected abstract void Init();

    // 初期設定
    private void Awake()
    {
        // ScriptableObjectからステータスを取得
        InputStatus();
        // 敵の種類に応じて必要な初期化を実行
        Init();
        // アニメーターの初期化
        status.m_animator = gameObject.GetComponent<Animator>();
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
        status.m_health = initialStatus.health;
        status.m_power = initialStatus.power;
        status.m_speed = initialStatus.speed;
    }

    // アニメーションで更新されたオブジェクトの座標を保存
    private void PositionUpdate()
    {
        status.m_position = gameObject.transform.position;
    }

    private float GetDamage(float damageSize) { return damageSize / 2.0f; }
}
