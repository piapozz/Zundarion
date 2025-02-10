/*
* @file DamageObserver.cs
* @brief ダメージ処理のオブザーバークラス
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
