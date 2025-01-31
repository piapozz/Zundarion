using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class StageManager : MonoBehaviour
{
    public static StageManager instance { get; private set; } = null;

    [SerializeField]
    private StageData _stageData = null;

    [SerializeField]
    public Transform _playerAnchor = null;

    private int _nowBattle = -1;

    private List<Battle> _battleList = null;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //StartBattle(0);
        _battleList = new List<Battle>(_stageData.battleData.Length);
        for (int i = 0, max = _battleList.Count; i < max; i++)
        {
            int waveNum = _stageData.battleData[i].waveData.Length;
            _battleList[i] = new Battle(waveNum);
        }
    }

    /// <summary>
    /// waveの番号を指定して敵を出現させる
    /// </summary>
    /// <param name="waveNum"></param>
    public void StartBattle(int waveNum)
    {
        //WaveData waveData = _stageData.waveData[waveNum];
        //int characterNum = waveData.generateCharacterData.Length;
        //for (int i = 0; i < characterNum; i++)
        //{
        //    GameObject geneCharacter = waveData.generateCharacterData[i].characterPrefab;
        //    int anchorNum = waveData.generateCharacterData[i].generateAnchorNum;
        //    CharacterManager.instance.GenerateEnemy(geneCharacter, _battles[waveNum].anchors[anchorNum]);
        //}
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
