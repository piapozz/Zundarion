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
    /// wave�̔ԍ����w�肵�ēG���o��������
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
    /// �E�F�[�u�����ɐi�߂�
    /// </summary>
    public void NextWave()
    {
        // �G�̃��X�g����Ȃ玟�̃E�F�[�u�ɐi��
        //if (!IsEmpty(CharacterManager.instance.))
        _nowBattle++;
        StartBattle(_nowBattle);
    }
}
