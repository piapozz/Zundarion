/*
* @file CrteateDamageEffect.cs
* @brief ダメージのエフェクトを生成する
* @author ishihara
* @date 2025/2/02
*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrteateDamageEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject _damageEffect;

    [SerializeField]
    private GameObject _prent;

    float elapsedTime = 0.0f;

    public static readonly int POOL_COUNT = 100;
    List<GameObject> _effectList = null;

    // Start is called before the first frame update
    void Start()
    {
        _effectList = new List<GameObject>(POOL_COUNT);
        // プール
        for (int i = 0; i < POOL_COUNT; i++)
        {
            var effect = Instantiate(_damageEffect, new Vector3(0, 0, 0), Quaternion.identity, _prent.transform);
            effect.SetActive(false);
            _effectList.Add(effect);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (elapsedTime > 0.1f) 
        {
            elapsedTime = 0.0f;
            int damage = Random.Range(50000, 10000000);

            // 生成
            Create(new Vector3(500.0f,200.0f,0.0f), damage);
        }
        elapsedTime += Time.deltaTime;
    }

    private void Create(Vector3 position, int damage)
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
