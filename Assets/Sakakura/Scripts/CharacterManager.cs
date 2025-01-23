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
    private GameObject playerOrigin = null;

    public GameObject playerObject { get; private set; } = null;

    public List<GameObject> characterList { get; private set; } = null;

    private void Awake()
    {
        instance = this;
        characterList = new List<GameObject>(CHARACTER_MAX);
        for (int i = 0, max = CHARACTER_MAX; i < max; i++)
        {
            characterList.Add(null);
        }
    }

    private void Start()
    {
        playerObject = GeneratePlayer(playerOrigin, StageManager.instance._playerAnchor);
    }

    /// <summary>
    /// �v���C���[�𐶐�����
    /// </summary>
    public GameObject GeneratePlayer(GameObject geneCharacter, Transform geneTransform)
    {
        return GenerateCharacter(geneCharacter, geneTransform);
    }

    /// <summary>
    /// �G�𐶐�����
    /// </summary>
    /// <param name="generateNum"></param>
    public GameObject GenerateEnemy(GameObject geneCharacter, Transform geneTransform)
    {
        return GenerateCharacter(geneCharacter, geneTransform);
    }

    /// <summary>
    /// �L�����N�^�[�𐶐�����֐�
    /// </summary>
    /// <param name="character"></param>
    private GameObject GenerateCharacter(GameObject geneCharacter, Transform geneTransform)
    {
        GameObject geneObj = null;
        BaseCharacter character = geneCharacter.GetComponent<BaseCharacter>();
        int useID = GetEmptyID();
        if (useID < 0)
        {
            useID = characterList.Count;
            character.Initialize(useID);
            geneObj = Instantiate(geneCharacter);
            characterList.Add(geneObj);
        }
        else
        {
            character.Initialize(useID);
            geneObj = Instantiate(geneCharacter);
            characterList[useID] = geneObj;
        }
        character.SetTransform(geneTransform);
        return geneObj;
    }

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
