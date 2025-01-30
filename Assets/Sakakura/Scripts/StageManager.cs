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

    [SerializeField]
    private AnchorList[] _battles = null;

    [System.Serializable]
    public class AnchorList
    {
        public Transform[] anchors;
    }

    private int _nowBattle = -1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //StartBattle(0);
    }

    /// <summary>
    /// waveの番号を指定して敵を出現させる
    /// </summary>
    /// <param name="waveNum"></param>
    public void StartBattle(int waveNum)
    {
        WaveData waveData = _stageData.waveData[waveNum];
        int characterNum = waveData.generateCharacterData.Length;
        for (int i = 0; i < characterNum; i++)
        {
            GameObject geneCharacter = waveData.generateCharacterData[i].characterPrefab;
            int anchorNum = waveData.generateCharacterData[i].generateAnchorNum;
            CharacterManager.instance.GenerateEnemy(geneCharacter, _battles[waveNum].anchors[anchorNum]);
        }
    }

    /// <summary>
    /// ウェーブを次に進める
    /// </summary>
    public void NextWave()
    {
        // 敵のリストが空なら次のウェーブに進む
        //if (!IsEmpty(CharacterManager.instance.))
        _nowBattle++;
        StartBattle(_nowBattle);
    }
}
