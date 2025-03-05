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
    public EffectManager effectManager = null;

    [SerializeField] private GameObject eyeLeft = null, eyeRight = null;
    [SerializeField] public LightEffectController lightEffectController = null;

    private const float _HIT_IMPACT_RATIO = 10;

    private void Start()
    {
        selfAnimator = GetComponent<Animator>();
        player = CharacterManager.instance.player;
        effectManager = EffectManager.instance;
    }

    public void SetAnimatorTrigger(string triggerName) { selfAnimator?.SetTrigger(triggerName); }

    public void SetAnimatorBool(string boolName, bool value) { selfAnimator?.SetBool(boolName, value); }

    // ブレイク値を変動させる
    public void ReceiveBreakPoint(float breakSize) { breakPoint -= breakSize; }

    public override bool IsPlayer() { return false; }

    public override void TakeDamage(float damageSize, float strength)
    {
        base.TakeDamage(damageSize, strength);

        if (health <= 0)
        {
            selfAnimator.SetBool(_selfAnimationData.animationName[(int)EnemyAnimation.DYING], true);
            CharacterManager.instance.RemoveCharacterList(ID);
            StageManager.instance.CheckWaveFinish();
        }
        else if (GetDamage(strength, damageSize) > (healthMax / _HIT_IMPACT_RATIO))
        {
            // 一定の割合のダメージを受けたらひるむ
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

    //public void EyeEffectEvent(EffectGenerateData data)
    //{
    //    AudioManager audioManager = AudioManager.instance;

    //    EffectManager.instance.GenerateEffect(data, eyeLeft.transform);
    //    EffectManager.instance.GenerateEffect(data, eyeRight.transform);

    //    audioManager.PlaySE(SE.ENEMY_OMEN);
    //}

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
