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
        // �_���[�W�\�L
        UIManager.instance.GenerateDamageEffect(target.position, damage, Color.red);
        // SE�Đ�
        AudioManager.instance.PlaySE(SE.HIT);

        // �_���[�W�G�t�F�N�g����
    }
}
