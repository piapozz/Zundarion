using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : SystemObject
{
    public static FadeManager instance = null;

    /// <summary>暗転用黒テクスチャ</summary>
    private GameObject _fadeImage;
    private Material _fadeMaterial;

    public override void Initialize()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        if (this != instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        // フェードプレハブ取得
        _fadeImage = Resources.Load<GameObject>("Prefabs/Fade/FadeImage");
        GameObject fade = Instantiate(_fadeImage, transform);
        _fadeMaterial = fade.GetComponentInChildren<Image>().material;
        _fadeMaterial.SetFloat("_Transition", 1);
    }

    /// <summary>
    /// シーン遷移用コルーチン
    /// </summary>
    /// <param name='scene'>シーン名</param>
    /// <param name='interval'>暗転にかかる時間(秒)</param>
    public async UniTask TransScene(string scene, float interval)
    {
        //だんだん暗く
        float transition = 1;
        float time = 0;

        while (time < interval)
        {
            time += Time.deltaTime;
            transition = 1 - time / interval;
            _fadeMaterial.SetFloat("_Transition", transition);
            await UniTask.DelayFrame(1);
        }

        //シーン切替
        SceneManager.LoadScene(scene);

        //だんだん明るく
        transition = 1;
        time = 0;
        while (time < interval)
        {
            time += Time.deltaTime;
            transition = time / interval;
            _fadeMaterial.SetFloat("_Transition", transition);
            await UniTask.DelayFrame(1);
        }

        await UniTask.DelayFrame(1);
    }

}
