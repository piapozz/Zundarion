/*
 * @file EnemyUI.cs
 * @brief 敵UIの操作を行うクラス
 * @author sein
 * @date 2025/1/31
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour 
{
    private Image image = null;                 // UIとなるImage
    public BaseEnemy baseEnemy { get; protected set; } = null;         // 敵のクラス（情報）
    public float health { get; protected set; } = -1;                  // 敵の現在体力
    public float healthMax { get; protected set; } = -1;               // 敵の最大体力

    private void Start()
    {
        image = null;
        baseEnemy = null;
        health = -1;
        healthMax = -1;

        SetActive(false);
    }

    private void Update()
    {
        if (baseEnemy == null) return;

        UpdateUIPosition();
        UpdateHealth();
        UpdateImage();
    }

    /// <summary>
    /// セットアップを行う
    /// </summary>
    /// <param name="_baseEnemy"></param>
    /// <param name="_enemyUI"></param>
    public void Setup(BaseEnemy _baseEnemy, GameObject _enemyUI)
    {
        image = _enemyUI.GetComponent<Image>();
        baseEnemy = _baseEnemy.GetComponent<BaseEnemy>();
        health = _baseEnemy.GetComponent<BaseCharacter>().health;
        healthMax = _baseEnemy.GetComponent<BaseCharacter>().healthMax;
    }

    /// <summary>
    /// リセット関数
    /// </summary>
    public void Teardown()
    {
        image = null;
        baseEnemy = null;
        health = -1;
        healthMax = -1;

        SetActive(false);
    }

    public void UpdateImage()
    {
        image.fillAmount = health / healthMax;
    }

    private void UpdateHealth()
    {
        health = baseEnemy.health;
    }

    private void UpdateUIPosition()
    {
        SetScreenPosition(transform.position);
    }

    private void SetScreenPosition(Vector3 screenPosition) { transform.position = screenPosition; }

    public void SetActive(bool active) { gameObject.SetActive(active); }
}
