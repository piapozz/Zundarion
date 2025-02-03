/*
* @file EnemyUI.cs
* @brief �GUI�̑�����s���N���X
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
    /// ������
    /// </summary>
    public void Initialize()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// ���Z�b�g�֐�
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
