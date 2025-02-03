/*
* @file UIManager.cs
* @brief ゲームのUIを管理するクラス
* @author sein
* @date 2025/1/31
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 出来れば敵の最大体力を見てUIを伸ばす
// 体力が減るまでラグを作る
// Poolingで実装する

// ※Canvasを敵の数だけ生成するのは描画コストが増え、負荷につながる

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; } = null;

    private readonly int INITIAL_COUNT = 5;

    [SerializeField] private GameObject enemyUI;
    [SerializeField] private GameObject canvasWorldSpace;

    private CharacterManager characterManager = null;
    private Camera mainCamera = null;

    // 使用中のキャラクターリスト
    private List<BaseCharacter> useList = null;
    private List<BasePlayer> unusePlayer = null;
    private List<BaseEnemy> unuseEnemyList = null;

    private List<GameObject> useObjectList = null;
    private List<GameObject> unuseObjectList = null;

    Vector3 viewportPos;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        characterManager = CharacterManager.instance;
        mainCamera = CameraManager.instance.selfCamera;

        // 全てが空だったらあらかじめ生成する
        if (useObjectList == null && unuseObjectList == null)
        {
            useObjectList = new List<GameObject>();
            unuseObjectList = new List<GameObject>();

            // Canvasのtransformを取得
            var parent = canvasWorldSpace.transform;

            for (int i = 0, max = INITIAL_COUNT; i < max; i++)
            {
                AddEnemyUI(parent);
            }
        }
    }

    private void Update()
    {
        List<BaseCharacter> characterList = characterManager.characterList;

        for (int i = 0, max = characterList.Count; i < max; i++) 
        {
            //Debug.Log("temae");
            //if (characterList[i].GetComponent<BasePlayer>()) break;
            //Debug.Log("通った");
            //viewportPos = mainCamera.WorldToViewportPoint(characterList[i].transform.position + Vector3.up * 2f);

            //if (viewportPos.z > 0 && viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1)
            //{
            //    Debug.Log("aaaaa");
            //}
            //else
            //{
            //    Debug.Log("iii");
            //}
        }

        // 使用されるときに必要な情報を渡して表示する

        // もしUIがセットされていない敵が見つかったら
        if (true)
        {

        }

        // 使用されていないオブジェクトがあったら非表示にする
        for (int i = 0, max = unuseObjectList.Count; i < max; i++) 
        {
            // 非表示にされていないオブジェクトがあったら
            if(unuseObjectList[i].activeSelf != false)
            {
                SetActive(unuseObjectList[i], false);
            }
        }

        // UIを更新する
        for (int i = 0, max = useObjectList.Count; i < max; i++)
        {

            if(useObjectList[i] != null)
            {

            }
        }
    }

    // リストにUIを追加する
    private void AddEnemyUI(Transform transform)
    {
        // 子として生成
        unuseObjectList.Add(Instantiate(enemyUI, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, transform));
    }

    /// <summary>
    /// 指定されたオブジェクトのアクティブ切り替え関数
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(GameObject gameObj, bool active)
    {
        gameObj.SetActive(active);
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    time += Time.deltaTime;

    //    //// プレイヤーの体力をゲージで変動させる
    //    for (int i = 0; i < playerHealthBar.Length; i++)
    //    {
    //        // VariableBar(playerHealthBar[i], MAX_TIME, basePlayer[i].selfCurrentHealth);
    //        VariableBar(playerHealthBar[(int)TeamMember.MEMBER_1], zundaObject.selfCurrentHealth, 100.0f);
    //        VariableBar(playerHealthBar[(int)TeamMember.MEMBER_2], zunkoObject.selfCurrentHealth, 100.0f);
    //    }

    //    //// プレイヤーのスキルゲージを変動させる
    //    //for (int i = 0; i < playerSkillBar.Length; i++)
    //    //{
    //    //    VariableBar(playerSkillBar[i], MAX_TIME, MAX_TIME - time);
    //    //}

    //    //敵の体力をゲージで変動させる
    //    for (int i = 0; i < enemyHealthBar.Length; i++)
    //    {
    //        //VariableBar(enemyHealthBar[i], enemyBear.status.m_health, enemyBear.status.m_healthMax);
    //        //Debug.Log(enemyBear.status.m_health);
    //        //Debug.Log(enemyBear.status.m_healthMax);
    //    }

    //    //// 敵のブレイク値の溜まり具合を変動させる
    //    //for (int i = 0; i < enemyBreakBar.Length; i++)
    //    //{

    //    //}
    //}

}
