using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] HighLightController highLightController = null;
    [SerializeField] float time;

    // Start is called before the first frame update
    async void Start()
    {
        await UniTask.Delay(1000);
        highLightController.HighLight(time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
