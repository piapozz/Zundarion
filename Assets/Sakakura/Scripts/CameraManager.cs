using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; private set; } = null;

    /// <summary>カメラ</summary>
    [SerializeField]
    public Camera selfCamera;

    /// <summary>カメラ制御</summary>
    private CinemachineStateDrivenCamera _stateCam;

    /// <summary>フリールックカメラ</summary>
    private CinemachineFreeLook _freeLookCam;

    private void Awake()
    {
        instance = this;
        Initialize();
    }

    private void Start()
    {
        Transform setTransform = CharacterManager.instance.player.transform;
        Animator setAnimator = CharacterManager.instance.GetCharacter(0).selfAnimator;
        SetTransform(setTransform, setAnimator);
    }

    private void Initialize()
    {
        selfCamera = Camera.main;
        _stateCam = GetComponentInChildren<CinemachineStateDrivenCamera>();
        _freeLookCam = GetComponentInChildren<CinemachineFreeLook>();
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
