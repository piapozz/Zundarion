using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamageEvent : DamageObserver
{
    public void Initialize()
    {
    }
    public void OnDamage(Transform target, int damage)
    {
        // ダメージ表記
        UIManager.instance.GenerateDamageEffect(target.position, damage, Color.red);
        // SE再生
        AudioManager.instance.PlaySE(SE.HIT);

        // コンボを途切れさせる
        ComboManager.instance.BreakCombo();

        // ダメージエフェクト生成
    }
}
