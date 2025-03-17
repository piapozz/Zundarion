using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : SystemObject
{
    public static FadeManager instance = null;

    /// <summary>暗転用黒テクスチャ</summary>
    private GameObject _fadeImage;
    private Material _fadeMaterial;
    private Canvas _canvas;

    public override void Initialize()
    {
        if (instance == null)
        {
            instance = this;
        }
        // フェードプレハブ取得
        _fadeImage = Resources.Load<GameObject>("Prefabs/Fade/FadeImage");
        GameObject fade = Instantiate(_fadeImage, transform);

        _canvas = fade.GetComponent<Canvas>();
        _fadeMaterial = fade.GetComponentInChildren<Image>().material;
        _fadeMaterial.SetFloat("_Transition", 1);
        _canvas.enabled = false; // キャンバスを非表示
    }

    /// <summary>
    /// シーン遷移用コルーチン
    /// </summary>
    /// <param name='scene'>シーン名</param>
    /// <param name='interval'>暗転にかかる時間(秒)</param>
    public async UniTask TransScene(string scene, float interval)
    {
        _canvas.enabled = true; // キャンバスを表示

        // だんだん暗く
        float transition = 0;
        float time = 0;

        while (time < interval)
        {
            time += Time.deltaTime;
            transition = Mathf.Lerp(1, 0, time / interval);
            _fadeMaterial.SetFloat("_Transition", transition);
            await UniTask.Yield();
        }

        // シーン切替（非同期処理）
        await SceneManager.LoadSceneAsync(scene);

        // だんだん明るく
        transition = 1;
        time = 0;

        while (time < interval)
        {
            time += Time.deltaTime;
            transition = Mathf.Lerp(0, 1, time / interval);
            _fadeMaterial.SetFloat("_Transition", transition);
            await UniTask.Yield();
        }

        _canvas.enabled = false; // キャンバスを非表示
    }
}
