using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; private set; } = null;

    /// <summary>�J����</summary>
    [SerializeField]
    public Camera selfCamera;

    /// <summary>�J��������</summary>
    private CinemachineStateDrivenCamera _stateCam;

    /// <summary>�t���[���b�N�J����</summary>
    private CinemachineFreeLook _freeLookCam;

    void Start()
    {
        instance = this;
        selfCamera = Camera.main;
        _stateCam = GetComponentInChildren<CinemachineStateDrivenCamera>();
        _freeLookCam = GetComponentInChildren<CinemachineFreeLook>();
    }

    public void SetFreeCam(float xAxis, float yAxis)
    {
        _freeLookCam.m_XAxis.Value = xAxis;
        _freeLookCam.m_YAxis.Value = yAxis;
    }
}
