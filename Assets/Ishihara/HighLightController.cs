using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using static CommonModule;

public class HighLightController : MonoBehaviour
{
    public Color _color { get; private set; }
    [SerializeField] private float fadeInAlpha = 1.0f;
    [SerializeField] private float fadeOutAlpha = 0.0f;
    private Outline[] outline;

    private void Start()
    {
        outline = gameObject.GetComponentsInChildren<Outline>();
    }

    private void SetOutlineColor(Color color)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>(); // 子の Renderer も取得
        foreach (var renderer in renderers)
        {
            if (renderer == null) continue;

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetColor("_OutlineColor", color); // QuickOutline のシェーダープロパティ
            renderer.SetPropertyBlock(block);
        }
    }

    private void FadeIn(float fadeInTime)
    {
        DOVirtual.Float(0f, fadeInAlpha, fadeInTime, fadeInAlpha =>
        {
            Color color = _color;
            color.a = fadeInAlpha;
            SetOutlineColor(color); // すべての子オブジェクトの Renderer に適用
        });
    }

    private void FadeOut(float fadeOutTime)
    {
        DOVirtual.Float(1f, fadeOutAlpha, fadeOutTime, fadeOutAlpha =>
        {
            Color color = _color;
            color.a = fadeOutAlpha;
            SetOutlineColor(color); // すべての子オブジェクトの Renderer に適用
        });
    }

    public async UniTask HighLight(float sec)
    {
        float time = sec / 2;
        FadeIn(time);
        await WaitAction(time, FadeOut, time);
    }

    public void SetColor(Color color)
    {
        _color = color;
    }
}