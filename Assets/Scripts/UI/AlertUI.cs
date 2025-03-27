/*
 * @file WaveUI.cs
 * @brief ウェーブのUI管理
 * @author sakakura
 * @date 2025/3/24
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlertUI : MonoBehaviour
{
    [SerializeField]
    private Image alertImage = null;

    [SerializeField]
    private TextMeshProUGUI alertUIText = null;

    public void SetText(string alertText)
    {
        string setText = string.Format(alertText);
        alertUIText.text = setText;
    }
}
