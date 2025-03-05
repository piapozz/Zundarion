using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading;

using static CommonModule;
using static GameConst;

public abstract class BasePlayer : BaseCharacter
{
    // public //////////////////////////////////////////////////////////////////

    /// <summary>プレイヤーの初期設定</summary>
    public virtual void Setup()
    {

    }

    /// <summary>自身のゲームオブジェクト</summary>
    public GameObject selfGameObject { get; private set; }

    // protected //////////////////////////////////////////////////////////////////

    /// <summary>派生先による初期化</summary>
    protected abstract void Init();

    // private //////////////////////////////////////////////////////////////////

    /// <summary>入力された移動方向</summary>
    private Vector2 _inputMoveDir = Vector2.zero;

    /// <summary>移動に掛かる倍率</summary>
    private float _currentMultiplier = 1.0f;

    /// <summary>硬直中かどうか</summary>
    private bool _isStiff = false;

    /// <summary>移動硬直中かどうか</summary>
    private bool _isMoveStiff = false;

    /// <summary>プレイヤーの先行入力情報</summary>
    [SerializeField]
    private PreInput _selfPreInput = null;

    /// <summary>パリィのストック</summary>
    private int _parryStock = 0;

    /// <summary>回避のストック</summary>
    private int _avoidStock = 0;

    /// <summary>パリィのクールダウンタスク</summary>
    private UniTask _parryCoolDownTask;

    /// <summary>パリィのクールダウンキャンセルトークン</summary>
    private CancellationTokenSource _parryCTS = null;

    /// <summary>回避のクールダウンタスク</summary>
    private UniTask _avoidCoolDownTask;

    /// 回避のクールダウンキャンセルトークン
    private CancellationTokenSource _avoidCTS = null;

    private List<BaseCharacter> _parryList = null;

    private List<BaseCharacter> _avoidList = null;

    private const float _RUN_SPEED_RATE = 1.5f;         // 走る速度倍率
    private const float _ATTACK_SENS_RANGE = 10.0f;     // 攻撃感知範囲
    private const int _PARRY_COOL_DOWN_STOCK = 2;       // パリィクールダウンストック
    private const float _PARRY_COOL_DOWN_SECOND = 2.0f; // パリィクールダウン秒数
    private const int _AVOID_COOL_DOWN_STOCK = 2;       // 回避クールダウンストック
    private const float _AVOID_COOL_DOWN_SECOND = 2.0f; // 回避クールダウン秒数
    private const int _ATTACK_CAMERA_FRAME = 20;        // 攻撃時のカメラ遷移フレーム

    public static bool isPlayerDead = false;

    void Awake()
    {
        _selfPreInput.Initialize();

        _parryStock = _PARRY_COOL_DOWN_STOCK;
        _avoidStock = _AVOID_COOL_DOWN_STOCK;

        isPlayerDead = false;

        _parryList = new List<BaseCharacter>(5);
        _avoidList = new List<BaseCharacter>(5);

        Init();
    }

    private void Update()
    {
        // 移動処理
        MoveExecute();

        // 先行入力処理
        PreInputExecute();
    }

    /// <summary>
    /// アクションマップのMoveに登録されているキーが押されたときに入力値を取得
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        _inputMoveDir = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 移動実行処理
    /// </summary>
    private void MoveExecute()
    {
        selfAnimator.SetBool("Move", false);

        // 移動できないなら処理を抜ける
        if (_isStiff || _isMoveStiff) return;

        // 移動方向が入力されているなら
        if (_inputMoveDir.x != 0 || _inputMoveDir.y != 0)
        {
            selfAnimator.SetBool("Move", true);
        }
        else
        {
            _currentMultiplier = 1.0f;
            return;
        }

        // 回転し移動
        Rotate(AdjustMoveDir());
        Move(speed * _currentMultiplier);
    }

    /// <summary>
    /// 先行入力に応じて処理
    /// </summary>
    private void PreInputExecute()
    {
        InputType input = _selfPreInput.preInputType;
        // 硬直中か入力されてないなら処理しない
        if (_isStiff || input == InputType.None) return;

        // 各入力の処理
        switch (input)
        {
            case InputType.Run:
                AvoidExecute();
                break;
            case InputType.Attack:
                AttackExecute();
                break;
            case InputType.Parry:
                ParryExecute();
                break;
        }
        _selfPreInput.ClearRecord();
    }

    /// <summary>
    /// 回避アクションを実行
    /// </summary>
    /// <returns></returns>
    private void AvoidExecute()
    {
        if (_isStiff) return;

        // クールダウン中なら処理を抜ける
        if (CheckAvoidCoolDown()) return;

        // ジャスト回避になるか判定
        if (_avoidList.Count <= 0)
        {
            selfAnimator.SetTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.AVOID]);
            _currentMultiplier = _RUN_SPEED_RATE;
        }
        else
        {
            if (_avoidList[0] == null) return;

            TurnNearEnemy();
            // アニメーションをセット
            selfAnimator.SetTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.JUST_AVOID]);
            // スロー
            SlowManager.instance.SetSlow(AVOID_SLOW_SPEED, AVOID_SLOW_TIME);
        }
    }

    private bool CheckAvoidCoolDown()
    {
        if (_avoidStock <= 0) return true;

        _avoidStock--;
        _avoidCTS = new CancellationTokenSource();
        // クールダウン中ならキャンセル
        if (!_avoidCoolDownTask.Status.IsCompleted())
            _avoidCTS.Cancel();

        _avoidCoolDownTask = WaitAction(_AVOID_COOL_DOWN_SECOND, () => _avoidStock = _AVOID_COOL_DOWN_STOCK, _avoidCTS.Token);

        return false;
    }

    /// <summary>
    /// 入力情報から移動方向を調整
    /// </summary>
    /// <returns></returns>
    private Vector3 AdjustMoveDir()
    {
        // カメラの方向に基づいて入力ベクトルを修正
        Vector3 cameraForward = CameraManager.instance.selfCamera.transform.forward;
        Vector3 cameraRight = CameraManager.instance.selfCamera.transform.right;
        // カメラ方向に基づいた移動ベクトルを計算
        Vector3 adjustedMove = (cameraRight * _inputMoveDir.x + cameraForward * _inputMoveDir.y).normalized;
        return adjustedMove;
    }

    /// <summary>
    /// 攻撃する
    /// </summary>
    private void AttackExecute()
    {
        TurnNearEnemy();
        // アニメーション設定
        selfAnimator.SetTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.ATTACK]);
        // カメラを調整
        UniTask task = CameraManager.instance.SetFreeCam(transform.eulerAngles.y, 0.5f, _ATTACK_CAMERA_FRAME);
    }

    private void TurnNearEnemy()
    {
        // 近くの敵を取得し角度調整
        BaseCharacter character = CharacterManager.instance.GetNearCharacter(this, _ATTACK_SENS_RANGE);
        if (character == null) return;

        targetEnemy = character;
        TurnAround(targetEnemy.transform);
    }

    /// <summary>
    /// パリィする
    /// </summary>
    private void ParryExecute()
    {
        // パリィクールダウン中なら処理を抜ける
        if (CheckParryCoolDown()) return;

        // パリィになるか判定
        if (_parryList.Count <= 0)
        {
            // アニメーションをセット
            selfAnimator.SetTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.PARRY_MISS]);
        }
        else
        {
            if (_parryList[0] == null) return;
            // アニメーションをセット
            selfAnimator.SetTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.PARRY]);
            // プレイヤーを敵の方向に向ける
            targetEnemy = _parryList[0];
            TurnAround(targetEnemy.transform);
            // 通常カメラをリセット
            UniTask task = CameraManager.instance.SetFreeCam(transform.eulerAngles.y, 0.5f);
            // パリィ相手のアニメーションをひるみにする
            _parryList[0].SetImpact();
        }
    }

    public void AddParryList(BaseCharacter target)
    {
        if (_parryList.Exists(chara => chara == target)) return;
        _parryList.Add(target);
    }

    public void RemoveParryList(BaseCharacter target)
    {
        if (_parryList.Exists(chara => chara != target)) return;
        _parryList.Remove(target);
    }

    public void AddAvoidList(BaseCharacter target)
    {
        if (_avoidList.Exists(chara => chara == target)) return;
        _avoidList.Add(target);
    }

    public void RemoveAvoidList(BaseCharacter target)
    {
        if (_avoidList.Exists(chara => chara != target)) return;
        _avoidList.Remove(target);
    }

    /// <summary>
    /// パリィクールダウン処理
    /// </summary>
    /// <returns></returns>
    private bool CheckParryCoolDown()
    {
        if (_parryStock <= 0) return true;

        _parryStock--;
        _parryCTS = new CancellationTokenSource();
        // クールダウン中ならキャンセル
        if (!_parryCoolDownTask.Status.IsCompleted())
            _parryCTS.Cancel();

        _parryCoolDownTask = WaitAction(_PARRY_COOL_DOWN_SECOND, () => _parryStock = _PARRY_COOL_DOWN_STOCK, _parryCTS.Token);
        return false;
    }

    /// <summary>
    /// 完全硬直を設定
    /// </summary>
    /// <param name="second"></param>
    public void SetStiffEvent(int frame)
    {
        _isStiff = true;
        UniTask task = WaitAction(frame, () => _isStiff = false);
    }

    /// <summary>
    /// 移動硬直を設定
    /// </summary>
    /// <param name="frame"></param>
    public void SetMoveStiffEvent(int frame)
    {
        _isMoveStiff = true;
        UniTask task = WaitAction(frame, () => _isMoveStiff = false);
    }

    /// <summary>
    /// 硬直解除
    /// </summary>
    public void ClearStiff()
    {
        _isStiff = false;
        _isMoveStiff = false;
    }

    /// <summary>
    /// パリィ時のスローを設定
    /// </summary>
    public void SetSlowEvent()
    {
        SlowManager.instance.SetSlow(PARRY_SLOW_SPEED, PARRY_SLOW_TIME);
    }

    public void StartTimeEvent()
    {
        SlowManager.instance.StartTime();
    }

    public override bool IsPlayer() { return true; }

    public override void TakeDamage(float damageSize, float strength)
    {
        // 無敵かHPがないなら処理しない
        if (isInvincible || isDead) return;

        base.TakeDamage(damageSize, strength);

        if (health <= 0)
        {
            selfAnimator.SetTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.DIE]);
            CharacterManager.instance.RemoveCharacterList(ID);
            isDead = true;
        }
        else
        {
            // 先行入力をキャンセル
            _selfPreInput.ClearRecord();
            // ひるむ
            SetImpact();
        }
    }

    public override void SetImpact()
    {
        selfAnimator.SetTrigger(_selfAnimationData.animationName[(int)PlayerAnimation.IMPACT]);
    }

    public override void DeadEvent()
    {
        base.DeadEvent();
        isPlayerDead = true;
        UniTask task = FadeManager.instance.TransScene("GameResult", SCENE_FADE_TIME);
    }
}
