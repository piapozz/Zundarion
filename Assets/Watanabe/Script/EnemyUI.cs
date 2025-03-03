/*
 * @file EnemyUI.cs
 * @brief �GUI�̑�����s���N���X
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
    private Camera mainCamera = null;                                       // ���C���J����
    private RectTransform rectTransform;                                    // �I�u�W�F�N�g�̍��W
    public BaseEnemy baseEnemy { get; private set; } = null;              // �G�̃N���X�i���j
    public Vector3 enemyPosition { get; private set; } = Vector3.zero;    // �G�̍��W���Ǘ� 
    public float health { get; protected set; } = -1;                       // �G�̌��ݑ̗�
    public float healthMax { get; protected set; } = -1;                    // �G�̍ő�̗�

    private readonly float HEIGHT_OFFSET = 2.0f;                            // UI�̈ʒu�𒲐�

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
    /// �Z�b�g�A�b�v���s��
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
    /// ���Z�b�g�֐�
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
    /// �̗͂Ȃǂ̃o�[���X�V����
    /// </summary>
    public void UpdateImage()
    {
        imageHealth.fillAmount = health / healthMax;
    }

    /// <summary>
    /// �K�v�ȏ����X�V����
    /// </summary>
    private void UpdateStatus()
    {
        health = baseEnemy.health;
        // �G�̍��W���擾
        enemyPosition = baseEnemy.GetEnemyPosition();
    }


    /// <summary>
    /// �X�N���[�����W��n���ƌ����ڏ�̍��W���X�V�ł���
    /// </summary>
    /// <param name="_screenPosition"></param>
    private void UpdateUIPosition(Vector3 _screenPosition)
    {
        rectTransform.position = _screenPosition;
    }

    /// <summary>
    /// �I�u�W�F�N�g�̗L������؂�ւ�
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(bool active) { gameObject.SetActive(active); }
}
