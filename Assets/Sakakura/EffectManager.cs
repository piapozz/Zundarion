/*
 * @file EffectManager.cs
 * @brief エフェクトを管理する
 * @author sakakura
 * @date 2025/2/25
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EffectManager : SystemObject
{
    [SerializeField]
    private GameObject[] _effectObject = null;

    public static EffectManager instance = null;

    public override void Initialize()
    {
        instance = this;
    }

    public void GenerateEffect(EffectGenerateData data, Transform setTransform)
    {
        GameObject geneffect = Instantiate(_effectObject[data.ID], data.dir * data.length, Quaternion.identity, setTransform);

        // 座標設定
        Vector3 genPos = setTransform.position;
        float angle = setTransform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float length = data.length;
        float height = geneffect.transform.position.y;
        Vector3 offset = new Vector3(Mathf.Sin(angle) * length, height, Mathf.Cos(angle) * length);
        genPos += offset;
        geneffect.transform.position = genPos;
    }
}
