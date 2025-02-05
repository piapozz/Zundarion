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

    private List<BattleProcessor> _battleList = null;

    public Transform _startTrasform { get; private set; } = null;

    private void Awake()
    {
        instance = this;
        Initialize();
        
    }

    private void Initialize()
    {
        _stageData = _stageObject.GetStageData();
        _startTrasform = _stageObject.GetSpownTransform();
        _battleList = new List<BattleProcessor>(_stageObject.GetBattle());
        for (int i = 0, max = _battleList.Count; i < max; i++)
        {
            _battleList[i].Initialize(i, _stageData.battleData[i]);
        }
    }

    /// <summary>
    /// 戦闘を次に進める
    /// </summary>
    public void NextBattle()
    {
        _nowBattle++;
    }

    /// <summary>
    /// 次のバトルに行けるか判定
    /// </summary>
    /// <param name="battleNum"></param>
    /// <returns></returns>
    public bool CanNextBattle(int battleNum)
    {
        if (_nowBattle >= 0 && !_battleList[_nowBattle].isFinished)
            return false;

        if (battleNum != _nowBattle + 1)
            return false;

        return true;
    }
}
