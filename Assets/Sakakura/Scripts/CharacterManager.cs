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

    public List<GameObject> enemyList { get; private set; } = null;

    private void Awake()
    {
        instance = this;
        enemyList = new List<GameObject>(ENEMY_MAX);
        for (int i = 0, max = ENEMY_MAX; i < max; i++)
        {
            enemyList.Add(null);
        }
    }

    private void Start()
    {
        playerObject = GeneratePlayer(playerOrigin, StageManager.instance._playerAnchor);
    }

    /// <summary>
    /// プレイヤーを生成する
    /// </summary>
    public GameObject GeneratePlayer(GameObject geneCharacter, Transform geneTransform)
    {
        return GenerateCharacter(geneCharacter, geneTransform);
    }

    /// <summary>
    /// 敵を生成する
    /// </summary>
    /// <param name="generateNum"></param>
    public GameObject GenerateEnemy(GameObject geneCharacter, Transform geneTransform)
    {
        return GenerateCharacter(geneCharacter, geneTransform);
    }

    /// <summary>
    /// キャラクターを生成する関数
    /// </summary>
    /// <param name="character"></param>
    private GameObject GenerateCharacter(GameObject geneCharacter, Transform geneTransform)
    {
        GameObject geneObj = null;
        BaseCharacter character = null;
        int useID = GetEmptyID();
        if (useID < 0)
        {
            useID = enemyList.Count + 1;
            geneObj = Instantiate(geneCharacter);
            character = geneObj.GetComponent<BaseCharacter>();
            character.Initialize(useID);
            enemyList.Add(geneObj);
        }
        else
        {
            geneObj = Instantiate(geneCharacter);
            character = geneObj.GetComponent<BaseCharacter>();
            character.Initialize(useID);
            enemyList[useID] = geneObj;
        }
        character.SetTransform(geneTransform);
        return geneObj;
    }

    /// <summary>
    /// 空のキャラクターリストの番号を取得
    /// </summary>
    /// <returns></returns>
    private int GetEmptyID()
    {
        int ID = -1;
        for (int i = 0, max = enemyList.Count; i < max; i++)
        {
            if (enemyList[i] != null)
                continue;
            ID = i + 1;
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
        enemyList[ID] = null;
    }

    /// <summary>
    /// BaseCharacterクラスを取得する
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public BaseCharacter GetCharacter(int ID)
    {
        return enemyList[ID].GetComponent<BaseCharacter>();
    }
}
