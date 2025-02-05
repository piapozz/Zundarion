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
    private Image image = null;                 // UI�ƂȂ�Image
    public BaseEnemy baseEnemy { get; protected set; } = null;         // �G�̃N���X�i���j
    public float health { get; protected set; } = -1;                  // �G�̌��ݑ̗�
    public float healthMax { get; protected set; } = -1;               // �G�̍ő�̗�

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
    /// �Z�b�g�A�b�v���s��
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
    /// ���Z�b�g�֐�
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
