/*
 * @file BaseCharacter.cs
 * @brief キャラクター共通の情報や処理をまとめたベースクラス
 * @author sein
 * @date 2025/1/17
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

// 親クラスのpublicをインスペクター上に表示
#if UNITY_EDITOR
[CustomEditor(typeof(BaseCharacter))]
#endif

public abstract class BaseCharacter : MonoBehaviour
{
    [SerializeField] 
    private CharacterData charaData = null;       // キャラクターのデータ

    [SerializeField]
    private int _ID = -1;

    // キャラクターのステータス
    [SerializeField] protected float healthMax = -1;         // 最大体力
    [SerializeField] protected float health = -1;            // 現在の体力
    protected float strength = -1;          // 攻撃力
    protected float defence = -1;           // 防御力
    protected float speed = -1;             // 速さ

    protected Vector3 position;        // 座標
    protected float multiplier;        // 倍率

    /// <summary>アニメーターコンポーネント</summary>
    public Animator selfAnimator = null;

    /// <summary>
    /// ScriptableObjectを使って初期化
    /// </summary>
    public void Initialize(int setID)
    {
        _ID = setID;
        healthMax = charaData.health;
        health = healthMax;
        strength = charaData.strength;
        defence = charaData.defence;
        speed = charaData.speed;
        selfAnimator = GetComponent<Animator>();
    }

    public void SetTransform(Transform setTransform)
    {
        transform.position = setTransform.position;
        transform.rotation = setTransform.rotation;
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="damageSize"></param>
    public void TakeDamage(float damageSize)
    {
        float damage = damageSize * defence * multiplier;

        if (damage > healthMax / 3)
        {
            // 例:大きくのけぞるアニメーションを再生
        }
        else if (damage > healthMax / 100)
        {
            // 例:のけぞるアニメーションを再生
        }

        // 負の数にならないように補正
        health = Mathf.Max(0, health - damage);

        health -= damage;
    }

    public float GetCharacterHealth() {  return health; }

    /// <summary>
    /// 当たり判定を生成する
    /// </summary>
    /// <param name="attackData"></param>
    public void CreateCollisionEvent(CharacterAttackData attackData)
    {
        // 生成
        CollisionManager.instance.CreateCollisionSphere(_ID, attackData, transform);
    }

    /// <summary>
    /// 目標の方向に振り向く
    /// </summary>
    /// <param name="target"></param>
    public void TurnAround(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        dir.y = position.y;
        Quaternion dirRot = Quaternion.LookRotation(dir);
        transform.rotation = dirRot;
    }

    /// <summary>
    /// アクションイベントを使って細かい移動を行う
    /// </summary>
    /// <param name="dir">移動させたい向き</param>
    /// <param name="frame">どれくらいのフレーム移動させるか</param>
    /// <param name="speed">移動させる速さ</param>
    /// <returns></returns>
    public async UniTask MoveFine(Vector3 dir, float frame, float speed)
    {
        float frameCount = 0.0f;
        Vector3 movePostion = transform.position;

        dir = dir.normalized;

        while (true)
        {
            // ループを抜ける処理
            if (frameCount >= frame) break;
            else { frameCount += Time.deltaTime; }

            // 指定された方向に移動
            movePostion += dir * speed * Time.deltaTime;
            transform.position = movePostion;

            // 1フレーム待ち
            await UniTask.DelayFrame(1);
        }
    }

    /// <summary>
    /// 前進させる
    /// </summary>
    public void Move()
    {
        // 向いている方向に移動
        Vector3 movePosition = transform.position + (transform.forward * speed * Time.deltaTime);

        transform.position = movePosition;
    }

    /// <summary>
    /// 角度に応じて向きを変更
    /// </summary>
    /// <param name="moveVec">移動方向</param>
    /// <param name="_transform">基準となる向き</param>
    public float Rotate(Vector2 moveVec, Transform _transform)
    {
        // 敵の前方と右方向を基準に移動ベクトルを計算
        Vector3 forward = _transform.forward;
        Vector3 right = _transform.right;

        // 敵の向きに基づいた移動ベクトルを計算
        Vector3 adjustedMove = (right * moveVec.x + forward * moveVec.y).normalized;

        // 向きを計算して更新
        float angle = Mathf.Atan2(adjustedMove.z, adjustedMove.x) * Mathf.Rad2Deg;

        return angle;
    }

}
