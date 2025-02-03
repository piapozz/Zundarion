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

        health -= damage;

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
        dir.y = position.y;
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

    //public async UniTask RotateEvent(RotateEventData eventData)
    //{
    //    float frameCount = 0.0f;
    //    Vector3 targetVec;
    //    GameObject player = CharacterManager.instance.playerObject;

    //    while (true)
    //    {
    //        Debug.Log("muiteru");
    //        // ループを抜ける条件
    //        if (frameCount >= eventData.frame) break;
    //        else { frameCount += 1; }

    //        targetVec = GetTargetVec(player.transform.position);

    //        // 移動ベクトルがゼロでない場合のみ処理を進める
    //        if (targetVec == Vector3.zero) break;

    //        // 目標の角度を計算
    //        float targetAngle = Mathf.Atan2(targetVec.z, targetVec.x) * Mathf.Rad2Deg;

    //        // 現在の回転角度から目標の角度へ補間
    //        Quaternion targetRotation = Quaternion.AngleAxis(targetAngle - 90, Vector3.down);
    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, eventData.speed * Time.deltaTime);

    //        // 1フレーム待ち
    //        await UniTask.DelayFrame(1);

    //    }
    //}

    /// <summary>
    /// 前進させる
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    public void Move(Vector3 dir, float speed)
    {
        // 向いている方向に移動
        Vector3 movePosition = transform.position + (transform.forward * speed * Time.deltaTime);
        // Vector3 movePosition = transform.position + (dir * speed * Time.deltaTime);

        transform.position = movePosition;
    }

    /// <summary>
    /// 向きたい方向に徐々に向かせる
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    public void Rotate(Vector3 dir, float speed)
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
    /// 体力を渡す
    /// </summary>
    /// <returns></returns>
    public float GetCharacterHealth() { return health; }

    public abstract bool IsPlayer();

}
