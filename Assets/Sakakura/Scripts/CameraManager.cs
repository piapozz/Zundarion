using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SystemObject
{
    public static CameraManager instance { get; private set; } = null;

    /// <summary>カメラ</summary>
    public Camera selfCamera;

    /// <summary>カメラ制御</summary>
    private CinemachineStateDrivenCamera _stateCam;

    /// <summary>フリールックカメラ</summary>
    private CinemachineFreeLook _freeLookCam;

    public override void Initialize()
    {
        instance = this;
        selfCamera = Camera.main;

        Transform setTransform = CharacterManager.instance.player.transform;
        Animator setAnimator = CharacterManager.instance.GetCharacter(0).selfAnimator;

        _stateCam = GetComponentInChildren<CinemachineStateDrivenCamera>();
        _freeLookCam = GetComponentInChildren<CinemachineFreeLook>();

        SetTransform(setTransform, setAnimator);
    }

    private void SetTransform(Transform setTransform, Animator setAnimator)
    {
        _stateCam.Follow = setTransform;
        _stateCam.LookAt = setTransform;
        _stateCam.m_AnimatedTarget = setAnimator;

    }

    public void SetFreeCam(float xAxis, float yAxis)
    {
        _freeLookCam.m_XAxis.Value = xAxis;
        _freeLookCam.m_YAxis.Value = yAxis;
    }
}
