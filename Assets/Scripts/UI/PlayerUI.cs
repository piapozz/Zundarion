using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image imageHealth = null;
    public float health { get; protected set; } = -1;                       // �G�̌��ݑ̗�
    public float healthMax { get; protected set; } = -1;                    // �G�̍ő�̗�
    public PlayerCharacter basePlayer { get; protected set; }

    private void Update()
    {
        UpdateStatus();
        UpdateImage(imageHealth);
    }

    /// <summary>
    /// �Z�b�g�A�b�v���s��
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
    /// ���Z�b�g�֐�
    /// </summary>
    public void Teardown()
    {
        basePlayer = null;
        health = -1;
        healthMax = -1;
    }

    /// <summary>
    /// �̗͂Ȃǂ̃o�[���X�V����
    /// </summary>
    public void UpdateImage(Image image)
    {
        image.fillAmount = health / healthMax;
    }

    /// <summary>
    /// �K�v�ȏ����X�V����
    /// </summary>
    private void UpdateStatus()
    {
        health = basePlayer.health;
    }
}
