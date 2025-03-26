/*
 * @file WaveUI.cs
 * @brief ウェーブのUI管理
 * @author sakakura
 * @date 2025/3/24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [SerializeField]
    private Image waveImage = null;

    [SerializeField]
    private TextMeshProUGUI waveText = null;

    public void SetText(int waveCount, int enemyCount)
    {
        string setText = string.Format("Wave{0}\nEnemy{1}", waveCount, enemyCount);
        waveText.text = setText;
    }
}
