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

    private static readonly int ENEMY_MAX = 10;

    [SerializeField]
    private GameObject playerOrigin = null;

    public GameObject playerObject { get; private set; } = null;

    public List<GameObject> characterList { get; private set; } = null;

    private void Awake()
    {
        instance = this;
        characterList = new List<GameObject>(ENEMY_MAX + 1);
        for (int i = 0, max = ENEMY_MAX + 1; i < max; i++)
        {
            characterList.Add(null);
        }
    }
    
    private void Start()
    {
        GeneratePlayer(playerOrigin, StageManager.instance._playerAnchor);
    }

    /// <summary>
    /// プレイヤーを生成する
    /// </summary>
    public void GeneratePlayer(GameObject genBase, Transform genTransform)
    {
        GameObject genCharacter = GenerateCharacter(genBase, genTransform);
        playerObject = genCharacter;
    }

    /// <summary>
    /// 敵を生成する
    /// </summary>
    /// <param name="generateNum"></param>
    public void GenerateEnemy(GameObject genBase, Transform genTransform)
    {
        GameObject genCharacter = GenerateCharacter(genBase, genTransform);
    }

    /// <summary>
    /// キャラクターを生成する
    /// </summary>
    /// <param name="genBase"></param>
    /// <param name="genTransform"></param>
    /// <returns></returns>
    private GameObject GenerateCharacter(GameObject genBase, Transform genTransform)
    {
        GameObject genCharacter = null;
        BaseCharacter character = null;
        int useID = GetEmptyID();
        if (useID < 0)
        {
            useID = characterList.Count;
            genCharacter = Instantiate(genBase);
            character = genCharacter.GetComponent<BaseCharacter>();
            character.Initialize(useID);
            characterList.Add(genCharacter);
        }
        else
        {
            genCharacter = Instantiate(genBase);
            character = genCharacter.GetComponent<BaseCharacter>();
            character.Initialize(useID);
            characterList[useID] = genCharacter;
        }
        character.SetTransform(genTransform);
        return genCharacter;
    }

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
        if (characterList[ID] == null) return null;

        return characterList[ID].GetComponent<BaseCharacter>();
    }
}
