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
    private Image image = null;
    private int characterID = -1;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// リセット関数
    /// </summary>
    public void Teardown()
    {
        characterID = -1;
    }

    public void UpdateHealth()
    {

    }

    public void SetScreenPosition(Vector3 screenPosition) { transform.position = screenPosition; }
    public void SetCharacterID(int ID) {  characterID = ID; }
}
