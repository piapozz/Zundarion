using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleProcessor : MonoBehaviour
{
    private int _battleNum = -1;
    // 開始してるか
    private bool _isStart = false;
    // 終了しているか
    public bool isFinished { get; private set; } = false;
    // 今何ウェーブ目か
    private int _waveCount = -1;
    // 今何体か
    private List<GameObject> _enemyList = null;
    // バトルデータ
    private BattleData _battleData = null;

    [SerializeField]
    private Transform[] _anchor = null;

    public void Initialize(int setBattleNum, BattleData setBattleData)
    {
        _battleNum = setBattleNum;
        _battleData = setBattleData;
    }

    private void Update()
    {
        if (!_isStart || isFinished) return;

        if (_waveCount >= _battleData.waveData.Length)
            isFinished = true;

        if (CharacterManager.instance.IsAliveEnemy()) return;

        _waveCount++;
        if (_waveCount >= _battleData.waveData.Length)
            isFinished = true;
        else
            StartWave(_battleData.waveData[_waveCount]);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isStart) return;

        if (other.tag != "Player") return;

        if (!StageManager.instance.CanNextBattle(_battleNum)) return;
        _isStart = true;
        StageManager.instance.NextBattle();
    }

    /// <summary>
    /// 指定したウェーブを開始する
    /// </summary>
    /// <param name="waveData"></param>
    private void StartWave(WaveData waveData)
    {
        int charaCount = waveData.generateCharacterData.Length;
        for (int i = 0; i < charaCount; i++)
        {
            GameObject genObject = waveData.generateCharacterData[i].characterPrefab;
            int genAnchorNum = waveData.generateCharacterData[i].generateAnchorNum;
            Transform genTransform = _anchor[i];
            CharacterManager.instance.GenerateEnemy(genObject, genTransform);
        }
    }

    /// <summary>
    /// ウェーブを進める
    /// </summary>
    public void NextBattle()
    {
        _waveCount++;
        StartWave(_battleData.waveData[_waveCount]);
    }

    public Transform[] GetAnchors()
    {
        return _anchor;
    }
}
