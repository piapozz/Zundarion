using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance { get; private set; } = null;

    private int _nowBattle = -1;

    [SerializeField]
    public StageObject _stageObject = null;

    private StageData _stageData = null;

    public Transform _startTrasform { get; private set; } = null;

    private void Awake()
    {
        instance = this;
        Initialize();
    }

    private void Start()
    {
        StartBattle(0);
    }

    private void Initialize()
    {
        _stageData = _stageObject.GetStageData();
        _startTrasform = _stageObject.GetSpownTransform();
    }

    public void StartBattle(int battleNum)
    {
        WaveData waveData = _stageData.battleData[battleNum].waveData[0];
        int waveCount = waveData.generateCharacterData.Length;
        for (int i = 0; i < waveCount; i++)
        {
            GameObject genObject = waveData.generateCharacterData[i].characterPrefab;
            int genAnchorNum = waveData.generateCharacterData[i].generateAnchorNum;
            Transform genTransform = _stageObject.GetAnchors(battleNum)[genAnchorNum];
            CharacterManager.instance.GenerateEnemy(genObject, genTransform);
        }
    }

    /// <summary>
    /// ウェーブを次に進める
    /// </summary>
    public void NextBattle()
    {
        // 敵のリストが空なら次のウェーブに進む
        //if (!IsEmpty(CharacterManager.instance.))
        _nowBattle++;
        StartBattle(_nowBattle);
    }
}
