/*
* @file DamageObserver.cs
* @brief �_���[�W�����̃I�u�U�[�o�[�N���X
* @author ishihara
* @date 2025/2/10
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DamageObserver
{
    public void OnDamage(Vector3 position);
}
