using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SystemObject
{
    public static StageManager instance { get; private set; } = null;

    private int _nowBattle = -1;

    [SerializeField]
    private GameObject _stageOrigin = null;

    private StageObject _stageObject = null;

    private StageData _stageData = null;

    private List<BattleProcessor> _battleList = null;

    public int battleCount { get; private set; } = 0;

    public Transform startTrasform { get; private set; } = null;

    public override void Initialize()
    {
        instance = this;

        GameObject genObj = Instantiate(_stageOrigin);
        _stageObject = genObj.GetComponent<StageObject>();
        _stageData = _stageObject.GetStageData();
        startTrasform = _stageObject.GetSpownTransform();
        _battleList = new List<BattleProcessor>(_stageObject.GetBattle());
        battleCount = _battleList.Count;
        for (int i = 0; i < battleCount; i++)
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
