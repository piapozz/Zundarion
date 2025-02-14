/**
 * @file SlowManager.cs
 * @brief �X���[�Ǘ��N���X
 * @author sakakura
 * @date 2025/2/10
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

using static CommonModule;

public class SlowManager : SystemObject
{
    public static SlowManager instance = null;

    /// <summary>�W���̃^�C���X�P�[��</summary>
    private float _DEFAULT_TIME_SCALE = 1.0f;

    /// <summary>���݂̃^�C���X�P�[��</summary>
    private float _currentTimeScale = 0;

    private CancellationTokenSource _slowCTS = null;

    private UniTask _slowTask;

    public override void Initialize()
    {
        instance = this;
        _slowCTS = new CancellationTokenSource();
        StartTime();
    }

    /// <summary>
    /// ��莞�Ԃ̃X���[��ݒ肷��
    /// </summary>
    /// <param name="setScale"></param>
    /// <param name="setSecond"></param>
    /// <returns></returns>
    public void SetSlow(float setScale, float setSecond)
    {
        if (!SetTimeScale(setScale)) return;

        _slowTask = WaitAction(setSecond, StartTime);
    }

    /// <summary>
    /// �w��̃^�C���X�P�[����ݒ肷��
    /// </summary>
    /// <param name="setScale"></param>
    private bool SetTimeScale(float setScale)
    {
        // �f�t�H���g�ݒ�łȂ������݂̎��Ԃ��ݒ�����x���ꍇ�͏������Ȃ�
        if (setScale != _DEFAULT_TIME_SCALE && _currentTimeScale < setScale) return false;

        // �X���[���̃^�X�N���I�����Ă��Ȃ��ꍇ�̓L�����Z��
        if (!_slowTask.Status.IsCompleted())
            CancelSlowTask();

        Time.timeScale = setScale;
        _currentTimeScale = setScale;
        
        return true;
    }

    /// <summary>
    /// �X���[�^�X�N�̃L�����Z��
    /// </summary>
    private void CancelSlowTask()
    {
        _slowCTS.Cancel();
        _slowCTS = new CancellationTokenSource();
    }

    public void StopTime()
    {
        SetTimeScale(0);
    }

    public void StartTime()
    {
        SetTimeScale(_DEFAULT_TIME_SCALE);
    }
}
