using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    [SerializeField]
    private StageData _stageData = null;

    [SerializeField]
    private Battle[] _battles = null;

    [SerializeField]
    private Transform _spownAnchor = null;

    [System.Serializable]
    private class Battle
    {
        [SerializeField]
        private Transform[] _anchors;

        public Transform[] GetAnchors()
        {
            return _anchors;
        }
    }

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
}
