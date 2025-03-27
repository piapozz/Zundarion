/*
 * @file TutorialUI.cs
 * @brief 操作説明のUI管理
 * @author sakakura
 * @date 2025/3/24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class TutorialUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _tutorialText = null;

    private bool _isController = false;

    private void Start()
    {
        SetTutorial(_isController);
    }

    private void LateUpdate()
    {
        if (!CheckChengeInput()) return;

        SetTutorial(_isController);
    }

    private bool CheckChengeInput()
    {
        if (_isController && (Gamepad.current == null))
        {
            _isController = false;
            return true;
        }

        if (!_isController && (Gamepad.current != null))
        {
            _isController = true;
            return true;
        }

        return false;
    }

    private void SetTutorial(bool isController)
    {
        if (isController)
        {
            string setText = "シールドはパリィで破壊！\n移動：Lスティック\n攻撃：Bボタン\n回避：Rボタン\nパリィ：Lボタン";
            _tutorialText.text = setText;
        }
        else
        {
            string setText = "シールドはパリィで破壊！\n移動：WASD\n攻撃：左クリック\n回避：Shift\nパリィ：E";
            _tutorialText.text = setText;
        }
    }
}
