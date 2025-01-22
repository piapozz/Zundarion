using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance { get; private set; } = null;

    [SerializeField]
    private StageData _stageData = null;

    [SerializeField]
    public Transform _playerAnchor = null;

    [SerializeField]
    private Transform[] _generateAnchor = null;

    private int _nowWave = -1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartWave(0);
    }

    public void StartWave(int waveNum)
    {
        _nowWave = waveNum;

        for (int i = 0, max = _stageData.waveData.Length; i < max; i++)
        {
            WaveData waveData = _stageData.waveData[waveNum];
            GameObject geneCharacter = waveData.generateCharacterData[i].characterPrefab;
            int anchorNum = waveData.generateCharacterData[i].generateAnchorNum;
            CharacterManager.instance.GenerateEnemy(geneCharacter, _generateAnchor[anchorNum]);
        }
    }
}
