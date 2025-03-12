/*
 * @file BaseEnemy.cs
 * @brief 敵のベースクラス
 * @author sein
 * @date 2025/1/17
 */

using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BaseEnemy : BaseCharacter
{
    public BasePlayer player { get; protected set; } = null;    // プレイヤー
    public float breakPoint { get; protected set; } = -1;       // ブレイク値

    public Vector3 position;

    public Vector3 targetVec;

    public IEnemyState enemyState = null;

    [SerializeField] private GameObject eyeLeft = null, eyeRight = null;
    [SerializeField] public LightEffectController lightEffectController = null;

    private const float _IMPACT_HEALTH_RATIO = 0.5f;   // 最大怯み値の体力比率(0～1)

    public override void Initialize(int setID)
    {
        base.Initialize(setID);

        player = CharacterManager.instance.player;
        targetEnemy = player;
    }

    public void SetAnimatorTrigger(string triggerName) { selfAnimator?.SetTrigger(triggerName); }

    public void SetAnimatorBool(string boolName, bool value) { selfAnimator?.SetBool(boolName, value); }

    // ブレイク値を変動させる
    public void ReceiveBreakPoint(float breakSize) { breakPoint -= breakSize; }

    public override bool IsPlayer() { return false; }

    protected override void OnDead()
    {
        base.OnDead();

        selfAnimator.SetBool(_selfAnimationData.animationName[(int)EnemyAnimation.DYING], true);
        StageManager.instance.CheckWaveFinish();
    }

    protected override void TakeImpact(float impact)
    {
        base.TakeImpact(impact);

        if (impactValue >= healthMax * _IMPACT_HEALTH_RATIO)
        {
            impactValue = 0;
            SetImpact();
        }
    }

    public override void SetImpact()
    {
        selfAnimator.SetTrigger(_selfAnimationData.animationName[(int)EnemyAnimation.HIT_BACK_HIGH]);
    }

    public void ChangeState(IEnemyState newState)
    {
        if (enemyState == newState || newState == null) return;
        if(enemyState != null)enemyState.Exit(this);
        enemyState = newState;
        enemyState.Enter(this);
    }

    public void EyeEffectEvent(EffectGenerateData data)
    {
        AudioManager audioManager = AudioManager.instance;

        EffectManager.instance.GenerateEffect(data, eyeLeft.transform);
        EffectManager.instance.GenerateEffect(data, eyeRight.transform);

        audioManager.PlaySE(SE.ENEMY_OMEN);
    }

    public void EyeEffectEvent(float sec)
    {
        AudioManager audioManager = AudioManager.instance;

        lightEffectController.SetTransform(sec);
        
        audioManager.PlaySE(SE.ENEMY_OMEN);
    }

    public void EnemyAction()
    {
        if (enemyState != null)
        {
            enemyState.Execute(this);
        }
    }

    public void EnemyExit()
    {
        enemyState.Exit(this);
    }

    public Vector3 GetEnemyPosition() { return transform.position; }
}
