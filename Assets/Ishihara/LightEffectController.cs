using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightEffectController : MonoBehaviour
{
    public ScreenLineRenderFeature renderFeature;
    public Transform enemyTransform;
    private Camera mainCamera;

    // �����̈ʒu�����炩�ɑJ�ڂ����邽�߂̕ϐ�
    private Vector2 currentLightSource;
    public float transitionSpeed = 5.0f;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera == null || enemyTransform == null) return;

        // �G�̃��[���h���W���X�N���[�����W�ɕϊ�
        Vector3 screenPos = mainCamera.WorldToViewportPoint(enemyTransform.position);
        // �����ʒu���X�V
        renderFeature.SetLightSource(screenPos);
    }
}
