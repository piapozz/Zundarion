using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using static EnemyBase;

// 敵クラスの親
// 基礎ステータスと使用されるAIはScriptableObjectでいじれるように作る
// UpdateEnemy関数を子クラスで編集してAIを作る

public abstract class EnemyBase : MonoBehaviour
{
    public InitialStatus initialStatus;     // ScriptableObjectのステータス情報
    public CollisionAction collisionAction; // ScriptableObjectの当たり判定情報
    private PlayerManager playerManager;     // プレイヤーの情報を管理する

    protected IEnemyState actionState;      // ステート

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
        public Vector3 m_relativePosition;  // プレイヤーとの距離を測って行動する
        public Quaternion m_toPlayerAngle;  // 敵から見たプレイヤーの方向
        public Vector3 m_forward;

        public int m_state;                         // 敵の状態
        public GameObject m_gameObject;             // ゲームオブジェクト
        public GameObject m_playerObject;           // Playerのデータ
        public Animator m_animator;                 // キャラクターに使われているAnimator
        public AnimatorStateInfo m_animatorState;   // アニメーション再生時間の長さを取得するための変数
        public CollisionAction m_collisionAction;

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

    protected EnemyStatus status;           // 構造体を生成
    protected int oldState;                 // ステータスの初期化

    // 敵の行動
    protected abstract void UpdateEnemy();

    // 状況に応じてステータス変更処理
    protected abstract void Init();

    // 初期設定
    private void Awake()
    {
        // アニメーターの初期化
        status.m_animator = gameObject.GetComponent<Animator>();
        // 当たり判定の生成に使うGameObjectを初期化
        status.m_gameObject = this.gameObject;
        // プレイヤーを取得 ※GameObject.Find()は重いらしいので使うなら初期化などのタイミングで一括
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        status.m_collisionAction = collisionAction;

        status.m_forward = Vector3.forward;

        // ステータスを初期化
        oldState = (int)EnemyBase.EnemyStatus.ActionState.STATE_IDLE;
        // ScriptableObjectからステータスを取得
        InputStatus();
        // 敵の種類に応じて必要な初期化を実行
        Init();
    }

    // 敵の処理
    private void Update()
    {
        // アニメーションの状態を取得
        status.m_animatorState = status.m_animator.GetCurrentAnimatorStateInfo(0);

        // 自身の位置情報を更新
        PositionUpdate();

        // プレイヤーーの更新
        status.m_playerObject = playerManager.GetActiveChara();

        // プレイヤーとの相対座標を取得
        GetRelativePosition();

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
    //public float visionDistance = 10.0f;  // 視界の距離
    //public float visionAngle = 45.0f;     // 視野角
    //public int rayCount = 50;             // 扇形に飛ばすRayの本数 
     
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

    // 渡された番号でステートを変更
    protected void SetState(int stateNum)
    {
        // ステートを保持しておく
        oldState = stateNum;

        switch (stateNum)
        {
            case (int)EnemyBase.EnemyStatus.ActionState.STATE_IDLE:
                actionState = new BearIdle();
                break;
            case (int)EnemyBase.EnemyStatus.ActionState.STATE_FOUND:
                actionState = new BearFound();
                break;
            case (int)EnemyBase.EnemyStatus.ActionState.STATE_TRACKING:
                actionState = new BearTracking();
                break;
            case (int)EnemyBase.EnemyStatus.ActionState.STATE_TURN:
                // actionState = new BearTurn();
                break;
            case (int)EnemyBase.EnemyStatus.ActionState.STATE_ATTACK:
                actionState = new BearAttack();
                break;
            case (int)EnemyBase.EnemyStatus.ActionState.STATE_ATTACK_UNIQUE:
                actionState = new BearAttackUnique();
                break;
            case (int)EnemyBase.EnemyStatus.ActionState.STATE_DOWN:
                actionState = new BearDown();
                break;
            case (int)EnemyBase.EnemyStatus.ActionState.STATE_DEAD:
                actionState = new BearDead();
                break;

        }
    }

    // アニメーションで更新されたオブジェクトの座標を保存
    private void PositionUpdate()
    {
        status.m_position = gameObject.transform.position;
    }

    // プレイヤーとの相対距離を取得
    private void GetRelativePosition()
    {
        status.m_relativePosition = status.m_playerObject.transform.position - status.m_position;
    }

    // ダメージを受ける
    public void ReceiveDamage(float damageSize) { status.m_health -= damageSize * status.m_defence * status.m_multiplier; }

    // ブレイク値を変動させる
    public void ReceiveBreakPoint(float breakSize) { status.m_break -= breakSize; }
}
