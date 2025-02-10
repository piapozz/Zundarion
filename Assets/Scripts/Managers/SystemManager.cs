using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    [SerializeField] private List<SystemObject> originSystemList = null; 
    private List<SystemObject> systemObjectList = null;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0, max = originSystemList.Count; i < max; i++)
        {
            SystemObject origin = originSystemList[i];
            if (origin == null) continue;

            SystemObject createObj = Instantiate(origin, transform);
            createObj.Initialize();
        }
    }
}
