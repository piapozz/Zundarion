/*
 * @file StageData.cs
 * @brief ステージのデータ
 * @author sakakura
 * @date 2025/1/22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Stage/StageData")]
public class StageData : ScriptableObject
{
    public BattleData[] battleData;
}
