using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static GameConst;

public class BattleProcessor : MonoBehaviour
{
    private int _battleNum = -1;
    // 開始してるか
    private bool _isStart = false;
    // 終了しているか
    public bool isFinished { get; private set; } = false;
    // 今何ウェーブ目か
    private int _waveCount = -1;
    // バトルデータ
    private BattleData _battleData = null;

    [SerializeField]
    private Transform[] _anchor = null;

    [SerializeField]
    private GameObject _closeObject = null;

    public void Initialize(int setBattleNum, BattleData setBattleData)
    {
        _battleNum = setBattleNum;
        _battleData = setBattleData;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isStart) return;

        if (other.tag != "Player") return;

        if (!StageManager.instance.CanNextBattle(_battleNum)) return;
        StartBattle();
    }

    /// <summary>
    /// ウェーブを進める
    /// </summary>
    public void NextWave()
    {
        _waveCount++;
        if (_waveCount >= _battleData.waveData.Length)
            FinishBattle();
        else
            StartWave(_battleData.waveData[_waveCount]);
    }

    /// <summary>
    /// 指定したウェーブを開始する
    /// </summary>
    /// <param name="waveData"></param>
    private void StartWave(WaveData waveData)
    {
        // ウェーブのすべての敵を出現させる
        int charaCount = waveData.generateCharacterData.Length;
        for (int i = 0; i < charaCount; i++)
        {
            GameObject genObject = waveData.generateCharacterData[i].characterPrefab;
            int genAnchorNum = waveData.generateCharacterData[i].generateAnchorNum;
            Transform genTransform = _anchor[genAnchorNum];
            CharacterManager.instance.GenerateEnemy(genObject, genTransform);
        }
        // UI
        UIManager.instance.SetWaveUI(_waveCount + 1, charaCount);
        if (_waveCount == 2) UIManager.instance.EnemyPopup(PopupText.INFO_BARRIER);
    }

    /// <summary>
    /// 戦闘を開始する
    /// </summary>
    private void StartBattle()
    {
        _isStart = true;
        _closeObject.SetActive(true);
        NextWave();
        StageManager.instance.NextBattle();
        UIManager.instance.EnemyPopup(PopupText.INFO_PARRY);
    }

    /// <summary>
    /// 戦闘を終了する
    /// </summary>
    private void FinishBattle()
    {
        isFinished = true;
        _closeObject.SetActive(false);
        if (StageManager.instance.battleCount <= _battleNum + 1)
            FadeManager.instance.TransScene("GameResult", SCENE_FADE_TIME);
    }

    public Transform[] GetAnchors()
    {
        return _anchor;
    }
}
