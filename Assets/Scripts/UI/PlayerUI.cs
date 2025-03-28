using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image imageHealth = null;
    public float health { get; protected set; } = -1;                       // 敵の現在体力
    public float healthMax { get; protected set; } = -1;                    // 敵の最大体力
    public PlayerCharacter basePlayer { get; protected set; }

    private void Update()
    {
        UpdateStatus();
        UpdateImage(imageHealth);
    }

    /// <summary>
    /// セットアップを行う
    /// </summary>
    /// <param name="_baseEnemy"></param>
    /// <param name="_enemyUI"></param>
    public void Setup(PlayerCharacter _basePlayer)
    {
        basePlayer = _basePlayer;
        health = _basePlayer.GetComponent<BaseCharacter>().health;
        healthMax = _basePlayer.GetComponent<BaseCharacter>().healthMax;
    }

    /// <summary>
    /// リセット関数
    /// </summary>
    public void Teardown()
    {
        basePlayer = null;
        health = -1;
        healthMax = -1;
    }

    /// <summary>
    /// 体力などのバーを更新する
    /// </summary>
    public void UpdateImage(Image image)
    {
        image.fillAmount = health / healthMax;
    }

    /// <summary>
    /// 必要な情報を更新する
    /// </summary>
    private void UpdateStatus()
    {
        health = basePlayer.health;
    }
}
