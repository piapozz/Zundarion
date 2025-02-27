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
        // �_���[�W�\�L
        UIManager.instance.GenerateDamageEffect(position, damage, Color.yellow);

        // �R���{�\�L
        ComboManager.instance.AddCombo();

        // SE�Đ�
        AudioManager.instance.PlaySE(AudioManager.SE.HIT);

        // �_���[�W�G�t�F�N�g����

    }
}
