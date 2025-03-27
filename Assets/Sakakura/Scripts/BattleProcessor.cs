using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static GameConst;

public class BattleProcessor : MonoBehaviour
{
    private int _battleNum = -1;
    // �J�n���Ă邩
    private bool _isStart = false;
    // �I�����Ă��邩
    public bool isFinished { get; private set; } = false;
    // �����E�F�[�u�ڂ�
    private int _waveCount = -1;
    // �o�g���f�[�^
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
    /// �E�F�[�u��i�߂�
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
    /// �w�肵���E�F�[�u���J�n����
    /// </summary>
    /// <param name="waveData"></param>
    private void StartWave(WaveData waveData)
    {
        // �E�F�[�u�̂��ׂĂ̓G���o��������
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
    /// �퓬���J�n����
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
    /// �퓬���I������
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
