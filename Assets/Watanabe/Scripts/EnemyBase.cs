using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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
        public float m_defence;             // 防御力
        public float m_break;               // 現在のブレイク値
        public float m_breakMax;            // ブレイク値の最大値
        public float m_multiplier;          // ブレイクした後のダメージ倍率
        public float m_distance;            // 敵の視認範囲
        public float m_vision;              // 敵の視野
        public bool m_dead;                 // 死亡状態
        public Vector3 m_position;          // 敵の座標
        public Vector3 m_positionNext;      // 敵の移動後予定の座標

        public IEnemyState m_state;                 // 敵の状態
        public GameObject m_gameObject;             // ゲームオブジェクト
        public GameObject m_playerObject;           // Playerのデータ
        public Animator m_animator;                 // キャラクターに使われているAnimator
        public AnimatorStateInfo m_animatorState;   // アニメーション再生時間の長さを取得するための変数

        public enum ActionState
        {
            STATE_IDLE = 0,                 // 待機状態
            STATE_FOUND,                    // 見つけたとき
            STATE_TRACKING,                 // 距離が離れたとき
            STATE_TURN,                     // 距離は近いが攻撃範囲から外れたとき
            STATE_ATTACK,                   // 通常攻撃
            STATE_ATTACK_UNIQUE,            // 特定の条件下でする攻撃
            STATE_DOWN,                     // ダウン状態
            STATE_DEAD,                     // 倒されたとき

            MAX
        }
    }

    //public float visionDistance = 10.0f;  // 視界の距離
    //public float visionAngle = 45.0f;     // 視野角
    //public int rayCount = 50;             // 扇形に飛ばすRayの本数

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
        // 当たり判定の生成に使うGameObjectを初期化
        status.m_gameObject = this.gameObject;
        // プレイヤーを取得 ※GameObject.Find()は重いらしいので使うなら初期化などのタイミングで一括
        status.m_playerObject = GameObject.Find("Player");
    }

    // 敵の処理
    private void Update()
    {
        // アニメーションの状態を取得
        status.m_animatorState = status.m_animator.GetCurrentAnimatorStateInfo(0);
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
        status.m_break = initialStatus.breakMax;
    }

    /*
    // Rayを飛ばして視野を作成
    protected void GetPlayerObject()
    {
        // Rayごとの角度
        float angleStep = (visionAngle * 2) / (rayCount - 1);
        // 扇形の左端から開始
        float startAngle = -visionAngle;

        // 設定された回数だけRay出す
        for (int i = 0; i < rayCount; i++)
        {
            // 現在の角度を計算
            float currentAngle = startAngle + (angleStep * i);

            // 角度に基づいてRayの方向を計算
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward;

            // Rayを飛ばす
            Ray enemyRay = new Ray(transform.position, direction);

            // 判定
            if (Physics.Raycast(enemyRay, out RaycastHit hit, visionDistance))
            {
                Debug.Log("当たった！");

                // タグの確認
                if (hit.collider.CompareTag("Player"))
                {
                    // タグがゲームオブジェクトだったら格納
                    status.m_playerObject = hit.collider.gameObject;
                }
            }
            else
            {
                // Rayが対象に当たらない場合
                Debug.DrawRay(transform.position, direction * visionDistance, Color.red);
            }
        }
    }
    */

    // アニメーションで更新されたオブジェクトの座標を保存
    private void PositionUpdate()
    {
        status.m_position = gameObject.transform.position;
    }

    // ダメージを受ける
    public void GetDamage(float damageSize) { status.m_health -= damageSize * status.m_defence * status.m_multiplier; }

    // ブレイク値を変動させる
    public void GetBreakPoint(float breakSize) { status.m_break -= breakSize; }
}
