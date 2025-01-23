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

        WaveData waveData = _stageData.waveData[waveNum];
        int characterNum = waveData.generateCharacterData.Length;
        for (int i = 0; i < characterNum; i++)
        {
            GameObject geneCharacter = waveData.generateCharacterData[i].characterPrefab;
            int anchorNum = waveData.generateCharacterData[i].generateAnchorNum;
            CharacterManager.instance.GenerateEnemy(geneCharacter, _generateAnchor[anchorNum]);
        }
    }
}
