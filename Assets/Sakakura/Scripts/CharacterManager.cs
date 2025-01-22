/*
* @file CharacterManager.cs
* @brief キャラクターの管理
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
    /// プレイヤーを生成する
    /// </summary>
    public void GeneratePlayer(GameObject geneCharacter, Transform geneTransform)
    {
        characterList.Clear();
        GenerateCharacter(geneCharacter, geneTransform);

    }

    /// <summary>
    /// 敵を生成する
    /// </summary>
    /// <param name="generateNum"></param>
    public void GenerateEnemy(GameObject geneCharacter, Transform geneTransform)
    {
        GenerateCharacter(geneCharacter, geneTransform);
    }

    /// <summary>
    /// キャラクターを生成する関数
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

    // FIX -1しか返ってこない
    /// <summary>
    /// 空のキャラクターリストの番号を取得
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
    /// キャラクターのリストからキャラを消す
    /// </summary>
    /// <param name="character"></param>
    public void RemoveCharacterList(int ID)
    {
        characterList[ID] = null;
    }

    /// <summary>
    /// BaseCharacterクラスを取得する
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public BaseCharacter GetCharacter(int ID)
    {
        return characterList[ID].GetComponent<BaseCharacter>();
    }
}
