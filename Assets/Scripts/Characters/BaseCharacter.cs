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

    /// <summary>ダメージオブザーバー</summary>
    [SerializeField]
    private DamageObserver _damageObserver = null;

    // キャラクターのステータス
    public float healthMax { get; protected set; } = -1;    // 最大体力
    public float health { get; protected set; } = -1;       // 現在の体力
    public float strength { get; protected set; } = -1;      // 攻撃力
    public float defence { get; protected set; } = -1;       // 防御力
    public float speed { get; protected set; } = -1;         // 速さ

    public float multiplier { get; protected set; } = 1;     // 倍率

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

    public void SetDamageObserver(DamageObserver observer)
    {
        _damageObserver = observer;
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="damageSize"></param>
    public virtual void TakeDamage(float damageSize)
    {
        int damage = (int)(damageSize * multiplier);

        health -= damage;

        _damageObserver.OnDamage(transform.position, damage);

        if (health <= 0)
            CharacterManager.instance.RemoveCharacterList(_ID);
    }

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
        dir.y = transform.position.y;
        Quaternion dirRot = Quaternion.LookRotation(dir);
        transform.rotation = dirRot;
    }

    /// <summary>
    /// アクションイベントを使って細かい移動を行う
    /// </summary>
    /// <param name="eventData">移動情報が入ったScriptObject</param>
    /// <returns></returns>
    public async UniTask MoveEvent(MoveEventData eventData)
    {
        int moveFrame = eventData.frame;
        float speed = eventData.speed;

        // directionをローカル座標系に変換
        Vector3 direction = transform.TransformDirection(eventData.dir.normalized);

        int frameCount = 0;
        while (moveFrame > frameCount)
        {
            // 指定された方向に移動
            transform.position += direction * speed / moveFrame;

            frameCount++;
            // 1フレーム待ち
            await UniTask.DelayFrame(1);
        }
    }

    /// <summary>
    /// 死亡イベント
    /// </summary>
    public virtual void DeadEvent()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// エフェクトを出すイベント
    /// </summary>
    /// <param name="ID"></param>
    public void EffectEvent(EffectGenerateData data)
    {
        EffectManager.instance.GenerateEffect(data, transform);
    }

    /// <summary>
    /// 前進させる
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    public void Move(float speed)
    {
        // 向いている方向に移動
        Vector3 movePosition = transform.position + (transform.forward * speed * Time.deltaTime);
        transform.position = movePosition;
    }

    /// <summary>
    /// 指定した方向に移動する
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="dir"></param>
    public void Move(float speed, Vector3 dir)
    {
        Vector3 localDir = transform.TransformDirection(dir);
        Vector3 movePosition = transform.position + (localDir * speed * Time.deltaTime);
        transform.position = movePosition;
    }

    /// <summary>
    /// 向きたい方向に徐々に向かせる
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    public void Rotate(Vector3 dir, float speed = GameConst.CHARACTER_ROTATE_SPEED)
    {
        // 移動ベクトルがゼロでない場合のみ処理を進める
        if (dir == Vector3.zero) return;

        // 目標の角度を計算
        float targetAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        // 現在の回転角度から目標の角度へ補間
        Quaternion targetRotation = Quaternion.AngleAxis(targetAngle - 90, Vector3.down);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime);
    }

    /// <summary>
    /// 目標への方向を取得
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public Vector3 GetTargetVec(Vector3 targetPos) { return (targetPos - transform.position).normalized; }

    /// <summary>
    /// 対象との相対距離を取得
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public Vector3 GetRelativePosition(Transform target) { return target.position - gameObject.transform.position; }

    /// <summary>
    /// 体力を渡す
    /// </summary>
    /// <returns></returns>
    public float GetCharacterHealth() { return health; }

    public abstract bool IsPlayer();

}
