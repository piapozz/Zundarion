/*
* @file UIManager.cs
* @brief ゲームのUIを管理するクラス
* @author sein
* @date 2025/1/31
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static CommonModule;

// 出来れば敵の最大体力を見てUIを伸ばす
// 体力が減るまでラグを作る
// Poolingで実装する

// ※Canvasを敵の数だけ生成するのは描画コストが増え、負荷につながる

public class UIManager : SystemObject
{
    public static UIManager instance { get; private set; } = null;

    private readonly int INITIAL_COUNT = 1;

    [SerializeField] private GameObject enemyUIObject;
    [SerializeField] private GameObject canvasWorldSpace;

    public Camera mainCamera { get; private set; } = null;
    private Transform parent = null;

    private List<EnemyUI> enemyUIList = null;
    private List<GameObject> useObjectList = null;
    private Queue<GameObject> unuseObjectQueue = null;

    Vector3 viewportPos ;

    public override void Initialize()
    {
        instance = this;

        // Canvasのtransformを取得
        parent = GameObject.Find("CanvasWorldSpace").transform;

        mainCamera = CameraManager.instance.selfCamera;

        // 全てが空だったらあらかじめ生成する
        if (useObjectList == null && unuseObjectQueue == null)
        {
            useObjectList = new List<GameObject>();
            unuseObjectQueue = new Queue<GameObject>();
            enemyUIList = new List<EnemyUI>();

            for (int i = 0, max = INITIAL_COUNT; i < max; i++)
            {
                unuseObjectQueue.Enqueue(Instantiate(enemyUIObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, parent));
            }
        }

    }

    public override void Proc()
    {
        // 不必要なUIを非表示にする
        for (int i = 0, max = enemyUIList.Count; i < max; i++)
        {
            if (enemyUIList[i] == null) break;

            Vector3 enemyPosition = enemyUIList[i].enemyPosition;

            viewportPos = mainCamera.WorldToViewportPoint(enemyPosition + Vector3.up * 2f);

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

        for (int i = 0, max = enemyUIList.Count; i < max; i++)
        {
            if (IsEmpty(enemyUIList) != true)
                ;
            if (enemyUIList[i].health <= 0)
            {
                RemoveEnemyUI(i);
                max -= 1;
            }
        }
    }

    public void AddEnemyUI(BaseEnemy baseEnemy)
    {
        GenerateEnemyUI(baseEnemy);
    }

    public void RemoveEnemyUI(int index)
    {
        enemyUIList[index].Teardown();

        unuseObjectQueue.Enqueue(useObjectList[index]);
        useObjectList.RemoveAt(index);
        enemyUIList.RemoveAt(index);
    }

    /// <summary>
    /// 敵とUIの結びつけを行う
    /// </summary>
    /// <param name="baseEnemy"></param>
    private void GenerateEnemyUI(BaseEnemy baseEnemy)
    {
        GameObject UIObject = null;
        EnemyUI enemyUI = null;

        // UIオブジェクトを再利用または生成する
        if (IsEmpty(unuseObjectQueue) != true)
        {
            UIObject = unuseObjectQueue.Dequeue();
        }
        else
        {
            UIObject = Instantiate(enemyUIObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, parent);
        }

        // クラスを取得
        enemyUI = UIObject.GetComponent<EnemyUI>();
        
        // 使用前のセットアップ 
        enemyUI.Setup(baseEnemy, UIObject);

        // 使用中リストに追加
        useObjectList.Add(UIObject);
        enemyUIList.Add(enemyUI);
    }
}
