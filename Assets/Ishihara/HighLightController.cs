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
        outline = gameObject.GetComponentsInChildren<Outline>(); //Outline���K�p���ꂽ�q���擾
    }

    private void FadeIn(float fadeInTime) //�t�F�[�h�C��
    {
        foreach (var outline in outline)
        {
            DOVirtual.Float(0f, fadeInAlpha, fadeInTime, fadeInAlpha =>
            {
                Color color = _color;
                color.a = fadeInAlpha;
                outline.OutlineColor = color;
            });
        }
        
    }

    private void FadeOut(float fadeOutTime) //�t�F�[�h�A�E�g
    {
        foreach (var outline in outline)
        {
            DOVirtual.Float(1f, fadeOutAlpha, fadeOutTime, fadeOutAlpha =>
            {
                Color color = _color;
                color.a = fadeOutAlpha;
                outline.OutlineColor = color;
            });
        }
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