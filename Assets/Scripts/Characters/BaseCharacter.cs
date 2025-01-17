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
using static UnityEditor.Experimental.GraphView.GraphView;

// 親クラスのpublicをインスペクター上に表示
#if UNITY_EDITOR
[CustomEditor(typeof(BaseCharacter))]
#endif

public abstract class BaseCharacter : MonoBehaviour
{
    [SerializeField] CharacterData charaData;       // キャラクターのデータ

    /// <summary>プレイヤーの移動データ</summary>
    [SerializeField]
    private CollisionAction _collisionAction = null;

    // キャラクターのステータス
    protected float healthMax;         // 最大体力
    protected float health;            // 現在の体力
    protected float strength;          // 攻撃力
    protected float defence;           // 防御力
    protected float speed;             // 速さ

    protected Vector3 position;        // 座標
    protected float multiplier;        // 倍率

    /// <summary>
    /// ScriptableObjectを使って初期化
    /// </summary>
    private void Awake()
    {
        healthMax = charaData.health;
        health = healthMax;
        strength = charaData.strength;
        defence = charaData.defence;
        speed = charaData.speed;
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

    public void CreateCollisionEvent(CollisionAction.CollisionLayer layer, Vector3 genOffset, float radius, float damage)
    {
        // 当たり判定を生成する
        CreateCollision.AttackData data = new CreateCollision.AttackData().zero();

        data.tagname = transform.tag;
        data.layer = _collisionAction.collisionLayers[(int)layer];
        data.position = transform.position + genOffset;
        data.radius = radius;
        data.time = 2;
        data.damage = damage;

        // 生成
        CreateCollision.instance.CreateCollisionSphere(gameObject, data);
    }
}
