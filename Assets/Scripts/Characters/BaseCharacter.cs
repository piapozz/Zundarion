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

using static CommonModule;
using static GameConst;

// 親クラスのpublicをインスペクター上に表示
#if UNITY_EDITOR
[CustomEditor(typeof(BaseCharacter))]
#endif

public abstract class BaseCharacter : MonoBehaviour
{
    [SerializeField] 
    private CharacterData charaData = null;       // キャラクターのデータ

    [SerializeField]
    protected int ID = -1;

    /// <summary>アニメーションのパラメーター情報</summary>
    [SerializeField]
    protected AnimationData _selfAnimationData;

    /// <summary>死亡フラグ</summary>
    protected bool isDead = false;

    /// <summary>無敵かどうか</summary>
    protected bool isInvincible = false;

    /// <summary>ターゲットの敵</summary>
    protected BaseCharacter targetEnemy = null;

    /// <summary>ダメージオブザーバー</summary>
    [SerializeField]
    private DamageObserver _damageObserver = null;

    [SerializeField]
    private GameObject[] _childObjectArray = null;

    [SerializeField]
    private Transform _effectAnchor = null;

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
    public virtual void Initialize(int setID)
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

    public void SetDamageObserver(DamageObserver observer)
    {
        _damageObserver = observer;
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="damageRatio"></param>
    public virtual void TakeDamage(float damageRatio, float sourceStrength)
    {
        // ダメージ処理
        int damage = GetDamage(sourceStrength, damageRatio);
        health = Mathf.Max(0, (health - damage));

        _damageObserver.OnDamage(_effectAnchor, damage);

        // カメラを揺らす
        CameraManager.instance.SetShake(CAMERA_SHAKE_TIME, CAMERA_SHAKE_GAIN);
    }

    /// <summary>
    /// 計算後のダメージを取得する
    /// </summary>
    /// <param name="sourceStrength"></param>
    /// <param name="damageRatio"></param>
    /// <returns></returns>
    protected int GetDamage(float sourceStrength, float damageRatio)
    {
        float defenceRatio = 100 / (defence + 100);
        float damage = damageRatio * sourceStrength;
        return (int)(damage * defenceRatio * multiplier);
    }

    /// <summary>
    /// ひるむ
    /// </summary>
    public virtual void SetImpact()
    {

    }

    /// <summary>
    /// アニメーションの再生時間を一定時間変える
    /// </summary>
    public void SetAnimationSpeed(float setSpeed, int frame)
    {
        selfAnimator.SetFloat("Speed", setSpeed);
        UniTask task = WaitAction(frame, () => selfAnimator.SetFloat("Speed", 1));
    }

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
        float length = eventData.length;
        // 接近する攻撃なら
        if (eventData.isApproach)
            length = GetMoveLength(eventData.minLength, eventData.maxLength);

        // directionをローカル座標系に変換
        Vector3 direction = transform.TransformDirection(eventData.dir.normalized);

        int frameCount = 0;
        while (moveFrame > frameCount)
        {
            // 指定された方向に移動
            transform.position += direction * length / moveFrame;
            //Vector3 nextPos = transform.position + direction * speed / moveFrame;
            //_rigidbody.MovePosition(nextPos);

            frameCount++;
            // 1フレーム待ち
            await UniTask.DelayFrame(1);
        }
    }

    /// <summary>
    /// 移動距離を取得
    /// </summary>
    /// <param name="minLength"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    private float GetMoveLength(float minLength, float maxLength)
    {
        Vector3 targetPosition = targetEnemy.transform.position;
        Vector3 selfPosition = transform.position;
        float distance = Vector3.Distance(targetPosition, selfPosition);

        return Mathf.Clamp((distance - minLength), 0, maxLength);
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
        EffectManager.instance.GenerateEffect(data, _effectAnchor);
    }

    /// <summary>
    /// 一定時間無敵にするイベント
    /// </summary>
    /// <param name="frame"></param>
    public void InvincibleEvent(int frame)
    {
        SetInvincible(true);
        UniTask task = WaitAction(frame, SetInvincible, false);
    }

    /// <summary>
    /// 無敵の可否設定
    /// </summary>
    /// <param name="setInvincible"></param>
    public void SetInvincible(bool setInvincible)
    {
        isInvincible = setInvincible;
    }

    /// <summary>
    /// 一定時間不可視にするイベント
    /// </summary>
    /// <param name="frame"></param>
    /// <returns></returns>
    public void InvisibleEvent(int frame)
    {
        InvisibleAll(false);
        UniTask task = WaitAction(frame, InvisibleAll, true);
    }

    /// <summary>
    /// 子オブジェクトのアクティブを切り替える
    /// </summary>
    /// <param name="visible"></param>
    private void InvisibleAll(bool visible)
    {
        for (int i = 0, max = _childObjectArray.Length; i < max; i++)
        {
            _childObjectArray[i].gameObject.SetActive(visible);
        }
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
        //_rigidbody.MovePosition(movePosition);
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
    public virtual void Rotate(Vector3 dir, float speed = CHARACTER_ROTATE_SPEED)
    {
        // 移動ベクトルがゼロでない場合のみ処理を進める
        if (dir == Vector3.zero) return;

        // 目標の角度を計算
        float targetAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(targetAngle - 90, Vector3.down);
        transform.rotation = targetRotation;
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
