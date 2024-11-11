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
    public GameObject instance;

    // 敵のステータス
    public struct Status
    {
        public int m_enemyNum;
        public float m_health;
        public float m_speed;
        public float m_strength;
    }

    public enum Patterns
    {
        NEUTRAL = 0,
        ATTACK,

        MAX
    }

    // 敵の行動
    protected abstract void UpdateEnemy();

    private void Awake()
    {
        Type aiComponentType = Type.GetType(enemyStatus.aiName);
        EnemyBase aiComponent = instance.AddComponent(aiComponentType) as EnemyBase;


    }



    // 敵の処理
    private void Update()
    {
        UpdateEnemy();
    }
}
