/**
 * @file SlowManager.cs
 * @brief スロー管理クラス
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

    /// <summary>標準のタイムスケール</summary>
    private float _DEFAULT_TIME_SCALE = 1.0f;

    /// <summary>現在のタイムスケール</summary>
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
    /// 一定時間のスローを設定する
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
    /// 指定のタイムスケールを設定する
    /// </summary>
    /// <param name="setScale"></param>
    private bool SetTimeScale(float setScale)
    {
        // デフォルト設定でないかつ現在の時間が設定よりも遅い場合は処理しない
        if (setScale != _DEFAULT_TIME_SCALE && _currentTimeScale < setScale) return false;

        // スロー中のタスクが終了していない場合はキャンセル
        if (!_slowTask.Status.IsCompleted())
            CancelSlowTask();

        Time.timeScale = setScale;
        _currentTimeScale = setScale;
        
        return true;
    }

    /// <summary>
    /// スロータスクのキャンセル
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
