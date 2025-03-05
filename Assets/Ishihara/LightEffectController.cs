using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightEffectController : MonoBehaviour
{
    public ScreenLineRenderFeature renderFeature;
    public Transform enemyTransform;
    private Camera mainCamera;

    // 光源の位置を滑らかに遷移させるための変数
    private Vector2 currentLightSource;
    public float transitionSpeed = 5.0f;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera == null || enemyTransform == null) return;

        // 敵のワールド座標をスクリーン座標に変換
        Vector3 screenPos = mainCamera.WorldToViewportPoint(enemyTransform.position);
        // 光源位置を更新
        renderFeature.SetLightSource(screenPos);
    }
}
