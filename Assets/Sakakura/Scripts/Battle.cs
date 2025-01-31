/*
 * @file StageData.cs
 * @brief バトルクラス
 * @author sakakura
 * @date 2025/1/31
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle
{
    public class Wave
    {
        private int _waveNum = -1;
        public bool _isFinish { get; private set; } = false;

        public Wave(int waveNum)
        {
            _waveNum = waveNum;
        }
    }

    public bool _isFinish { get; private set; } = false;

    public List<Wave> _waveList = null;

    public Battle(int waveNum)
    {
        _waveList = new List<Wave>(waveNum);
        for (int i = 0; i < waveNum; i++)
        {
            _waveList[i] = new Wave(i);
        }
    }
}
