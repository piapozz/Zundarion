using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CameraManager : SystemObject
{
    public static CameraManager instance { get; private set; } = null;

    /// <summary>カメラ</summary>
    public Camera selfCamera = null;

    /// <summary>カメラ制御</summary>
    [SerializeField]
    private CinemachineStateDrivenCamera _stateCam = null;

    /// <summary>フリールックカメラ</summary>
    [SerializeField]
    private CinemachineFreeLook _freeLookCam = null;

    /// <summary>衝撃を与える元</summary>
    [SerializeField]
    private CinemachineImpulseSource _impulseSource = null;

    /// <summary>カメラの自動旋回最大角度(0～1)</summary>
    private const float _CAMERA_AUTO_ROTATE_MAX = 30.0f;

    public override void Initialize()
    {
        instance = this;
        selfCamera = Camera.main;

        PlayerCharacter player = CharacterManager.instance.player;
        Transform setTransform = player.transform;
        Animator setAnimator = player.selfAnimator;
        SetTransform(setTransform, setAnimator);

        selfCamera.GetComponent<CameraViewFix>().Initialize(selfCamera);
    }

    private void SetTransform(Transform setTransform, Animator setAnimator)
    {
        _stateCam.Follow = setTransform;
        _stateCam.LookAt = setTransform;
        _stateCam.m_AnimatedTarget = setAnimator;
    }

    /// <summary>
    /// 指定の角度に指定の時間かけて回転
    /// </summary>
    /// <param name="setXAngle"></param>
    /// <param name="setYAngle"></param>
    /// <param name="setFrame"></param>
    public async UniTask SetFreeCam(float setXAngle, float setYAngle, int setFrame = 1)
    {
        if (setFrame <= 0) return;
        float diffXAngle = setXAngle - _freeLookCam.m_XAxis.Value;
        if (diffXAngle > 180) diffXAngle -= 360;
        else if (diffXAngle <= -180) diffXAngle += 360;
        diffXAngle = Mathf.Clamp(diffXAngle, -_CAMERA_AUTO_ROTATE_MAX, _CAMERA_AUTO_ROTATE_MAX);

        float diffYAngle = setYAngle - _freeLookCam.m_YAxis.Value;

        float xAxisLerp = diffXAngle / setFrame;
        float yAxisLerp = diffYAngle / setFrame;

        // 線形補間
        int elapsedFrame = 0;
        while (elapsedFrame < setFrame)
        {
            _freeLookCam.m_XAxis.Value += xAxisLerp;
            _freeLookCam.m_YAxis.Value += yAxisLerp;

            elapsedFrame++;
            await UniTask.DelayFrame(1);
        }
    }

    public void SetShake(float time, float gain)
    {
        CinemachineImpulseDefinition definition = _impulseSource.m_ImpulseDefinition;
        definition.m_TimeEnvelope.m_SustainTime = time;
        definition.m_AmplitudeGain = gain;
        _impulseSource.GenerateImpulse();
    }
}
