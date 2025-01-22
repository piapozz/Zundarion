/*
* @file CharacterManager.cs
* @brief �L�����N�^�[�̊Ǘ�
* @author sakakura
* @date 2025/1/22
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance { get; private set; } = null;

    private static readonly int CHARACTER_MAX = 10;

    [SerializeField]
    private GameObject _playerCharacter = null;

    public List<GameObject> characterList { get; private set; } = null;

    private void Awake()
    {
        instance = this;
        characterList = new List<GameObject>(CHARACTER_MAX);
    }

    private void Start()
    {
        GeneratePlayer(_playerCharacter, StageManager.instance._playerAnchor);
    }

    /// <summary>
    /// �v���C���[�𐶐�����
    /// </summary>
    public void GeneratePlayer(GameObject geneCharacter, Transform geneTransform)
    {
        characterList.Clear();
        GenerateCharacter(geneCharacter, geneTransform);

    }

    /// <summary>
    /// �G�𐶐�����
    /// </summary>
    /// <param name="generateNum"></param>
    public void GenerateEnemy(GameObject geneCharacter, Transform geneTransform)
    {
        GenerateCharacter(geneCharacter, geneTransform);
    }

    /// <summary>
    /// �L�����N�^�[�𐶐�����֐�
    /// </summary>
    /// <param name="character"></param>
    private void GenerateCharacter(GameObject geneCharacter, Transform geneTransform)
    {
        BaseCharacter character = gameObject.GetComponent<BaseCharacter>();
        int useID = GetEmptyID();
        if (useID < 0)
        {
            useID = characterList.Count;
            character.Initialize(useID);
            characterList.Add(Instantiate(geneCharacter));
        }
        else
        {
            character.Initialize(useID);
            characterList[useID] = Instantiate(geneCharacter);
        }
        character.SetTransform(geneTransform);
    }

    // FIX -1�����Ԃ��Ă��Ȃ�
    /// <summary>
    /// ��̃L�����N�^�[���X�g�̔ԍ����擾
    /// </summary>
    /// <returns></returns>
    private int GetEmptyID()
    {
        int ID = -1;
        for (int i = 0, max = characterList.Count; i < max; i++)
        {
            if (characterList[i] != null)
                continue;
            ID = i;
            break;
        }
        return ID;
    }

    /// <summary>
    /// �L�����N�^�[�̃��X�g����L����������
    /// </summary>
    /// <param name="character"></param>
    public void RemoveCharacterList(int ID)
    {
        characterList[ID] = null;
    }

    /// <summary>
    /// BaseCharacter�N���X���擾����
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public BaseCharacter GetCharacter(int ID)
    {
        return characterList[ID].GetComponent<BaseCharacter>();
    }
}
