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

    public BasePlayer player { get; private set; } = null;

    public List<BaseCharacter> characterList { get; private set; } = null;

    private void Awake()
    {
        instance = this;
        characterList = new List<BaseCharacter>(ENEMY_MAX + 1);
        for (int i = 0, max = ENEMY_MAX + 1; i < max; i++)
        {
            characterList.Add(null);
        }
    }
    
    private void Start()
    {
        GeneratePlayer(playerOrigin, StageManager.instance._startTrasform);
    }

    /// <summary>
    /// プレイヤーを生成する
    /// </summary>
    public void GeneratePlayer(GameObject genBase, Transform genTransform)
    {
        BaseCharacter genCharacter = GenerateCharacter(genBase, genTransform);
        player = genCharacter as BasePlayer;
    }

    /// <summary>
    /// 敵を生成する
    /// </summary>
    /// <param name="generateNum"></param>
    public void GenerateEnemy(GameObject genBase, Transform genTransform)
    {
        BaseCharacter genCharacter = GenerateCharacter(genBase, genTransform);

        UIManager.instance.AddEnemyUI(genCharacter as BaseEnemy);
    }

    /// <summary>
    /// キャラクターを生成する
    /// </summary>
    /// <param name="genBase"></param>
    /// <param name="genTransform"></param>
    /// <returns></returns>
    private BaseCharacter GenerateCharacter(GameObject genBase, Transform genTransform)
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
            characterList.Add(character);
        }
        else
        {
            genCharacter = Instantiate(genBase);
            character = genCharacter.GetComponent<BaseCharacter>();
            character.Initialize(useID);
            characterList[useID] = character;
        }
        character.SetTransform(genTransform);
        return character;
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

    /// <summary>
    /// エネミーがいるか判定
    /// </summary>
    /// <returns></returns>
    public bool IsAliveEnemy()
    {
        for (int i = 0, max = characterList.Count; i < max; i++)
        {
            if (characterList[i] == null || characterList[i].IsPlayer())
                continue;
            return true;
        }
        return false;
    }
}
