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
        // �_���[�W�\�L
        UIManager.instance.GenerateDamageEffect(target.position, damage, Color.yellow);

        // �R���{�\�L
        ComboManager.instance.AddCombo();

        // SE�Đ�
        AudioManager.instance.PlaySE(SE.HIT);

        // �_���[�W�G�t�F�N�g����
        EffectGenerateData data = new EffectGenerateData();
        data.ID = 2;
        data.length = 0;
        data.dir = Vector3.zero;
        EffectManager.instance.GenerateEffect(data, target);
    }
}
