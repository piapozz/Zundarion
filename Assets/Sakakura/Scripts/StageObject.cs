using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    [SerializeField]
    private StageData _stageData = null;

    [SerializeField]
    private BattleProcessor[] _battles = null;

    [SerializeField]
    private Transform _spownAnchor = null;

    public StageData GetStageData()
    {
        return _stageData;
    }

    public Transform GetSpownTransform()
    {
        return _spownAnchor;
    }

    public Transform[] GetAnchors(int battleNum)
    {
        return _battles[battleNum].GetAnchors();
    }

    public BattleProcessor[] GetBattle()
    {
        return _battles;
    }
}
