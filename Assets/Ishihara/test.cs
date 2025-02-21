using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        AudioManager.instance.PlayBGM(AudioManager.BGM.TITLE);
        AudioManager.instance.PlayBGM(AudioManager.BGM.MAIN);
        AudioManager.instance.PlayBGM(AudioManager.BGM.OTHER);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
