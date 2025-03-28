/*
 * @file UIManager.cs
 * @brief ゲームのUIを管理するクラス
 * @author sein
 * @date 2025/1/31
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

using static CommonModule;

// 出来れば敵の最大体力を見てUIを伸ばす
// 体力が減るまでラグを作る
// Poolingで実装する

// ※Canvasを敵の数だけ生成するのは描画コストが増え、負荷につながる

public class UIManager : SystemObject
{
    public static UIManager instance { get; private set; } = null;

    private readonly int INITIAL_COUNT = 1;
    public static readonly int POOL_COUNT = 100;

    [SerializeField] private GameObject damageEffect;

    [SerializeField] private GameObject playerUIObject;
    [SerializeField] private GameObject enemyUIObject;
    [SerializeField] private GameObject canvasWorldSpace;
    [SerializeField] private GameObject canvasOverlay;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private GameObject _waveUIObject = null;
    [SerializeField] private GameObject _popupUIObject = null;
    [SerializeField] private GameObject _tutorialObject = null;

    public Camera mainCamera { get; private set; } = null;
    private Transform parent = null;
    private Transform parentOverlay = null;

    private List<EnemyUI> enemyUIList = null;
    private List<GameObject> useObjectList = null;
    private Queue<GameObject> unuseObjectQueue = null;
    List<GameObject> _damageEffectList = null;
    private GameObject _waveCanvas = null;
    private GameObject _popupCanvas = null;
    private WaveUI _waveUI = null;
    private AlertUI _popupUI = null;

    private const float _WAVE_DISPLAY_SECOND = 3.0f;
    private const float _POPUP_DISPLAY_SECOND = 6.0f;

    Vector3 viewportPos;

    public override void Initialize()
    {
        instance = this;

        GameObject worldSpace = Instantiate(canvasWorldSpace, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        GameObject spaceOverlay = Instantiate(canvasOverlay, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

        // Canvasのtransformを取得
        mainCamera = CameraManager.instance.selfCamera;

        parent = worldSpace.transform;
        parentOverlay = spaceOverlay.transform;

        AddPlayerUI(CharacterManager.instance.player);

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
        _damageEffectList = new List<GameObject>(POOL_COUNT);
        // プール
        for (int i = 0; i < POOL_COUNT; i++)
        {
            var effect = Instantiate(damageEffect, new Vector3(0, 0, 0), Quaternion.identity, spaceOverlay.transform);
            effect.SetActive(false);
            _damageEffectList.Add(effect);
        }

        _waveCanvas = Instantiate(_waveUIObject, transform);
        _waveUI = _waveCanvas.GetComponent<WaveUI>();
        _waveCanvas.SetActive(false);

        _popupCanvas = Instantiate(_popupUIObject, transform);
        _popupUI = _popupCanvas.GetComponent<AlertUI>();
        _popupCanvas.SetActive(false);

        Instantiate(_tutorialObject, transform);
    }

    public override void Proc()
    {
        for (int i = 0; i < enemyUIList.Count; i++)
        {
            if (enemyUIList[i] == null) continue;

            Vector3 enemyPosition = enemyUIList[i].enemyPosition + Vector3.up * 2f;
            viewportPos = mainCamera.WorldToViewportPoint(enemyPosition);

            // カメラの後ろにある場合
            if (viewportPos.z < 0)
            {
                enemyUIList[i].SetActive(false);
                continue;
            }

            // 画面内判定
            bool isVisible = viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;
            // 障害物判定
            Vector3 direction = (enemyPosition - mainCamera.transform.position).normalized;

            float distance = Vector3.Distance(mainCamera.transform.position, enemyPosition);
            bool isObstructed = Physics.Raycast(mainCamera.transform.position, direction, distance, obstacleLayer);
            // bool shouldShowUI = isVisible && !isObstructed;

            bool shouldShowUI = !isObstructed;

            // UIの表示・非表示を設定
            enemyUIList[i].SetActive(shouldShowUI);
        }

        // HPが0のUIを削除
        for (int i = enemyUIList.Count - 1; i >= 0; i--)
        {
            if (enemyUIList[i].health <= 0)
            {
                RemoveEnemyUI(i);
            }
        }
    }

    public void AddEnemyUI(BaseEnemy baseEnemy)
    {
        GenerateEnemyUI(baseEnemy);
    }

    public void AddPlayerUI(PlayerCharacter basePlayer)
    {
        GeneratePlayerUI(basePlayer);
    }

    public void RemoveEnemyUI(int index)
    {
        enemyUIList[index].Teardown();

        unuseObjectQueue.Enqueue(useObjectList[index]);
        useObjectList.RemoveAt(index);
        enemyUIList.RemoveAt(index);
    }

    public void GenerateDamageEffect(Vector3 position, int damage, Color color)
    {
        position = Camera.main.WorldToScreenPoint(position);
        int activeNumber = -1;
        for (int i = 0; i < POOL_COUNT; i++)
        {
            if (_damageEffectList[i] == null || !_damageEffectList[i].activeSelf)
            {
                activeNumber = i;
                _damageEffectList[activeNumber].SetActive(true);
                break;
            }
        }

        if (activeNumber < 0) return;

        // 位置をずらす
        float dir = UnityEngine.Random.Range(0, 360);
        float length = UnityEngine.Random.Range(100, 150);

        SetPosition(_damageEffectList[activeNumber], dir, length, position);

        // ダメージの値を設定する
        var text = _damageEffectList[activeNumber].GetComponent<TextMeshProUGUI>();
        if (text == null) return;

        text.text = string.Format("{0}!!", damage);

        // 色を設定
        text.color = color;
        text.faceColor = Color.white;

        var damageEffect = _damageEffectList[activeNumber].GetComponent<DamageFont>();
        damageEffect.Execution();

    }

    private void SetPosition(GameObject effect, float dir, float length, Vector3 position)
    {
        float angleInRadians = dir * Mathf.Deg2Rad;
        Vector3 offset;
        offset.x = length * Mathf.Cos(angleInRadians);
        offset.y = length * Mathf.Sin(angleInRadians);
        offset.z = 0;

        effect.transform.position = offset + position;
    }
    private void GeneratePlayerUI(PlayerCharacter basePlayer)
    {
        PlayerUI playerUI = null;
        GameObject UIObject = Instantiate(playerUIObject, new Vector3(-150.0f, 80.0f, 0.0f), Quaternion.identity, parentOverlay);

        // クラスを取得
        playerUI = UIObject.GetComponent<PlayerUI>();
        playerUI.Setup(basePlayer);

        RectTransform rectTransform = UIObject.GetComponent<RectTransform>();

        // 左上に配置するための設定
        rectTransform.anchorMin = new Vector2(0, 1);  // 左上基準
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);      // 左上ピボット
        rectTransform.anchoredPosition = new Vector2(50, -50); // 左上から少しオフセット
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
        enemyUI.Setup(baseEnemy, UIObject);

        // 使用中リストに追加
        useObjectList.Add(UIObject);
        enemyUIList.Add(enemyUI);
    }

    public void SetWaveUI(int waveCount, int enemyCount)
    {
        _waveUI.SetText(waveCount, enemyCount);
        _waveCanvas.SetActive(true);
        UniTask task = WaitAction(_WAVE_DISPLAY_SECOND / 2, () => _waveCanvas.SetActive(false));
    }

    public void EnemyPopup(PopupText popupText)
    {
        _popupCanvas.SetActive(true);
        switch (popupText)
        {
            case PopupText.INFO_PARRY:
                _popupUI.SetText("赤いハイライトは回避で反撃、\n黄色いハイライトはパリィで反撃できる！");
                break;
            case PopupText.INFO_BARRIER:
                _popupUI.SetText("バリアはパリィすることで破ることができる！");
                break;
        }
        UniTask task = WaitAction(_POPUP_DISPLAY_SECOND, () => _popupCanvas.SetActive(false));
    }
}
