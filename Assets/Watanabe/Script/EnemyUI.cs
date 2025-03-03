/*
 * @file EnemyUI.cs
 * @brief 敵UIの操作を行うクラス
 * @author sein
 * @date 2025/1/31
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour 
{
    [SerializeField] private Image imageHealth = null;
    private Camera mainCamera = null;                                       // メインカメラ
    private RectTransform rectTransform;                                    // オブジェクトの座標
    public BaseEnemy baseEnemy { get; private set; } = null;              // 敵のクラス（情報）
    public Vector3 enemyPosition { get; private set; } = Vector3.zero;    // 敵の座標を管理 
    public float health { get; protected set; } = -1;                       // 敵の現在体力
    public float healthMax { get; protected set; } = -1;                    // 敵の最大体力

    private readonly float HEIGHT_OFFSET = 2.0f;                            // UIの位置を調節

    private void Start()
    {
        mainCamera = UIManager.instance.mainCamera;
        SetActive(false);
    }

    private void Update()
    {
        if (baseEnemy == null) return;

        UpdateStatus();

        Vector3 screenPosition = mainCamera.WorldToScreenPoint(baseEnemy.transform.position + Vector3.up * HEIGHT_OFFSET);

        UpdateUIPosition(screenPosition);

        UpdateImage();
    }

    /// <summary>
    /// セットアップを行う
    /// </summary>
    /// <param name="_baseEnemy"></param>
    /// <param name="_enemyUI"></param>
    public void Setup(BaseEnemy _baseEnemy, GameObject _enemyUI)
    {
        rectTransform = GetComponent<RectTransform>();

        baseEnemy = _baseEnemy;
        health = _baseEnemy.GetComponent<BaseCharacter>().health;
        healthMax = _baseEnemy.GetComponent<BaseCharacter>().healthMax;
    }

    /// <summary>
    /// リセット関数
    /// </summary>
    public void Teardown()
    {
        imageHealth = null;
        baseEnemy = null;
        health = -1;
        healthMax = -1;

        SetActive(false);
    }

    /// <summary>
    /// 体力などのバーを更新する
    /// </summary>
    public void UpdateImage()
    {
        imageHealth.fillAmount = health / healthMax;
    }

    /// <summary>
    /// 必要な情報を更新する
    /// </summary>
    private void UpdateStatus()
    {
        health = baseEnemy.health;
        // 敵の座標を取得
        enemyPosition = baseEnemy.GetEnemyPosition();
    }


    /// <summary>
    /// スクリーン座標を渡すと見た目上の座標を更新できる
    /// </summary>
    /// <param name="_screenPosition"></param>
    private void UpdateUIPosition(Vector3 _screenPosition)
    {
        rectTransform.position = _screenPosition;
    }

    /// <summary>
    /// オブジェクトの有効化を切り替え
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(bool active) { gameObject.SetActive(active); }
}
