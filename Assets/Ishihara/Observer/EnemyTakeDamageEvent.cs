using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamageEvent : DamageObserver
{
    public void Initialize()
    {

    }

    public void OnDamage(Vector3 position, int damage)
    {
        // ダメージ表記
        UIManager.instance.GenerateDamageEffect(position, damage, Color.yellow);

        // コンボ表記
        ComboManager.instance.AddCombo();

        // SE再生
        AudioManager.instance.PlaySE(AudioManager.SE.HIT);

        // ダメージエフェクト生成

    }
}
