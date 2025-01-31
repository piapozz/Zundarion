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
    /// wave�̔ԍ����w�肵�ēG���o��������
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
    /// �E�F�[�u�����ɐi�߂�
    /// </summary>
    public void NextBattle()
    {
        // �G�̃��X�g����Ȃ玟�̃E�F�[�u�ɐi��
        //if (!IsEmpty(CharacterManager.instance.))
        _nowBattle++;
        StartBattle(_nowBattle);
    }
}
