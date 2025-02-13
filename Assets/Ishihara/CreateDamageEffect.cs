/*
* @file CreateDamageEffect.cs
* @brief ダメージのエフェクトを生成する
* @author ishihara
* @date 2025/2/02
*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateDamageEffect : MonoBehaviour, DamageObserver
{
    public static readonly int POOL_COUNT = 100;
    List<GameObject> _effectList = null;

    public void Initialize(GameObject prefab, GameObject prent)
    {
        _effectList = new List<GameObject>(POOL_COUNT);
        // プール
        for (int i = 0; i < POOL_COUNT; i++)
        {
            var effect = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, prent.transform);
            effect.SetActive(false);
            _effectList.Add(effect);
        }
    }

    public void OnDamage(Vector3 position, int damage)
    {
        Camera.main.WorldToViewportPoint(position);
        int activeNumber = -1; 
        for (int i = 0; i < POOL_COUNT; i++)
        {
            if (_effectList[i] == null || !_effectList[i].activeSelf)
            {
                activeNumber = i;
                _effectList[activeNumber].SetActive(true);
                break;
            }
        }

        if (activeNumber < 0) return;

        // 位置をずらす
        float dir = Random.Range(0,360);
        float length = Random.Range(100,150);

        SetPosition(_effectList[activeNumber], dir, length, position);

        // ダメージの値を設定する
        var text = _effectList[activeNumber].GetComponent<TextMeshProUGUI>();
        if(text == null) return;

        text.text = string.Format("{0}!!", damage);

        var damageEffect = _effectList[activeNumber].GetComponent<DamageFont>();
        damageEffect.Execution();

    }

    private void SetPosition(GameObject effect, float dir, float length, Vector3 position)
    {
        float angleInRadians = dir * Mathf.Deg2Rad;
        Vector3 offset;
        offset.x = length * Mathf.Cos(angleInRadians);
        offset.y = length * Mathf.Sin(angleInRadians);
        offset.z = 0;

        effect.transform.position = offset + position;
    }
}
