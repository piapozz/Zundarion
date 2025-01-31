/*
 * @file BattleData.cs
 * @brief �퓬�̃f�[�^
 * @author sakakura
 * @date 2025/1/22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleData", menuName = "Stage/BattleData")]
public class BattleData : ScriptableObject
{
    public WaveData[] waveData;
}
