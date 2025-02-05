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

public class UIManager : SystemObject
{
    public static UIManager instance { get; private set; } = null;

    private readonly int INITIAL_COUNT = 3;

    [SerializeField] private GameObject enemyUIObject;
    [SerializeField] private GameObject canvasWorldSpace;

    private Camera mainCamera = null;
    private Transform parent = null;

    private List<EnemyUI> enemyUIList = null;
    private List<GameObject> useObjectList = null;
    private List<GameObject> unuseObjectList = null;

    Vector3 viewportPos;

    public override void Initialize()
    {
        instance = this;

        // Canvasのtransformを取得
        parent = canvasWorldSpace.transform;

        mainCamera = CameraManager.instance.selfCamera;

        // 全てが空だったらあらかじめ生成する
        if (useObjectList == null && unuseObjectList == null)
        {
            useObjectList = new List<GameObject>();
            unuseObjectList = new List<GameObject>();
            enemyUIList = new List<EnemyUI>();

            for (int i = 0, max = INITIAL_COUNT; i < max; i++)
            {
                unuseObjectList.Add(null);
                // unuseObjectList.Add(Instantiate(enemyUIObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, parent));
            }
        }
    }

    private void Update()
    {
        // 不必要なUIを非表示にする
        for (int i = 0, max = enemyUIList.Count; i < max; i++)
        {
            Debug.Log(i);
            if (enemyUIList[i] == null) break;

            viewportPos = mainCamera.WorldToViewportPoint(enemyUIList[i].baseEnemy.position + Vector3.up * 2f);

            // 画面に移ったら表示にする
            if (viewportPos.z > 0 && viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1)
            {
                enemyUIList[i].SetActive(true);
            }

            // 画面外に外れたら非表示にする
            else
            {
                enemyUIList[i].SetActive(false);
            }
        }
    }

    public void AddEnemyUI(BaseEnemy baseEnemy)
    {
        GenerateEnemyUI(baseEnemy);

    }

    public void RemoveEnemyUI(BaseEnemy baseEnemy)
    {

    }

    /// <summary>
    /// 敵とUIの結びつけを行う
    /// </summary>
    /// <param name="baseEnemy"></param>
    private void GenerateEnemyUI(BaseEnemy baseEnemy)
    {
        Debug.Log("UI生成");

        GameObject UIObject = null;
        EnemyUI enemyUI = null;

        // UIオブジェクトを再利用または生成する
        if (unuseObjectList[0] != null)
        {
            UIObject = unuseObjectList[0];
            unuseObjectList.RemoveAt(0);
            Debug.Log("未使用なものを使用する");
        }
        else UIObject = Instantiate(enemyUIObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, parent);

        // クラスを取得
        enemyUI = UIObject.GetComponent<EnemyUI>();
        
        // 使用前のセットアップ 
        enemyUI.Setup(baseEnemy, UIObject);

        // 使用中リストに追加
        useObjectList.Add(UIObject);
        enemyUIList.Add(enemyUI);
    }
}
