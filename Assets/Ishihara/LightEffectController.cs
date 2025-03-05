using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightEffectController : MonoBehaviour
{
    public ScreenLineRenderFeature renderFeature;
    public Transform enemyTransform;

    public void SetTransform(float sec)
    {
        if (Camera.main == null || enemyTransform == null) return;
        renderFeature.lineMaterial.SetFloat("_LineWidth", 0.001f);
        // 敵のワールド座標をスクリーン座標に変換
        Vector3 screenPos = Camera.main.WorldToViewportPoint(enemyTransform.position);
        renderFeature.SetLightSource(screenPos);
        CommonModule.WaitAction(sec, HideEffect);
    }

    private void HideEffect()
    {
        renderFeature.lineMaterial.SetFloat("_LineWidth", 0);
    }

    public void SetFeature(ScreenLineRenderFeature feature)
    {
        renderFeature = feature;
    }

}
