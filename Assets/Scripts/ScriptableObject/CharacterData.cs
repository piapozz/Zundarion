/*
 * @file CharacterData.cs
 * @brief �L�����N�^�[�̃X�e�[�^�X�̏����l�����߂�
 * @author sein
 * @date 2025/1/17
 */

using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;        // �L�����N�^�[��
    public float health;                // �̗�
    public float strength;              // �U����
    public float defence;               // �h���
    public float speed;                 // ����
}