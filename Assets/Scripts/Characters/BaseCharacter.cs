/*
 * @file BaseCharacter.cs
 * @brief キャラクター共通の情報や処理をまとめたベースクラス
 * @author sein
 * @date 2025/1/17
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 親クラスのpublicをインスペクター上に表示
#if UNITY_EDITOR
[CustomEditor(typeof(BaseCharacter))]
#endif

public abstract class BaseCharacter : MonoBehaviour
{
    [SerializeField] 
    private CharacterData charaData = null;       // キャラクターのデータ

    public int ID { get; private set; } = -1;

    // キャラクターのステータス
    protected float healthMax;         // 最大体力
    protected float health;            // 現在の体力
    protected float strength;          // 攻撃力
    protected float defence;           // 防御力
    protected float speed;             // 速さ

    protected Vector3 position;        // 座標
    protected float multiplier;        // 倍率

    /// <summary>アニメーターコンポーネント</summary>
    public Animator selfAnimator = null;

    /// <summary>
    /// ScriptableObjectを使って初期化
    /// </summary>
    public void Initialize(int setID)
    {
        ID = setID;
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
        CollisionManager.instance.CreateCollisionSphere(ID, attackData, transform);
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
}
