using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamageEvent : DamageObserver
{
    public void Initialize()
    {

    }

    public void OnDamage(Transform target, int damage)
    {
        // ダメージ表記
        UIManager.instance.GenerateDamageEffect(target.position, damage, Color.yellow);

        // コンボ表記
        ComboManager.instance.AddCombo();

        // SE再生
        AudioManager.instance.PlaySE(SE.HIT);

        // ダメージエフェクト生成
        EffectGenerateData data = new EffectGenerateData();
        data.ID = 2;
        data.length = 0;
        data.dir = Vector3.zero;
        EffectManager.instance.GenerateEffect(data, target);
    }
}
