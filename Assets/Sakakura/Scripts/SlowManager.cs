/**
 * @file SlowManager.cs
 * @brief �X���[�Ǘ��N���X
 * @author sakakura
 * @date 2025/2/10
 */

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowManager : MonoBehaviour
{
    public static SlowManager instance = null;

    /// <summary>
    /// �W���̃^�C���X�P�[��
    /// </summary>
    private float _DEFAULT_TIME_SCALE = 1.0f;

    private float _currentTimeScale = 0;

    public void Initialize()
    {
        instance = this;
        _currentTimeScale = _DEFAULT_TIME_SCALE;
    }

    /// <summary>
    /// �X���[��ݒ肷��
    /// </summary>
    /// <param name="setScale"></param>
    /// <param name="setSecond"></param>
    /// <returns></returns>
    public async UniTask SetSlow(float setScale, float setSecond)
    {
        SetTimeScale(setScale);

        float elapsedTime = 0;
        while (elapsedTime < setSecond)
        {
            elapsedTime += Time.deltaTime;

            await UniTask.WaitForSeconds(setScale);
        }

        SetTimeScale(_DEFAULT_TIME_SCALE);
    }

    /// <summary>
    /// �w��̃^�C���X�P�[����ݒ肷��
    /// </summary>
    /// <param name="setScale"></param>
    private void SetTimeScale(float setScale)
    {
        /// �^�C���X�P�[�����W���ȊO�̏ꍇ
        if (_currentTimeScale != _DEFAULT_TIME_SCALE) return;

        Time.timeScale = setScale;
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
