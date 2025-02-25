/*
 * @file EffectManager.cs
 * @brief エフェクトを管理する
 * @author sakakura
 * @date 2025/2/25
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SystemObject
{
    [SerializeField]
    private GameObject[] _effectObject = null;

    public static EffectManager instance = null;

    public override void Initialize()
    {
        instance = this;
    }

    public void GenerateEffect(int ID, Vector3 position)
    {
        Instantiate(_effectObject[ID], position, Quaternion.identity);
    }
}
