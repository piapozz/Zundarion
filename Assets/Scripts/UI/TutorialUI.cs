/*
 * @file TutorialUI.cs
 * @brief ���������UI�Ǘ�
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
            string setText = "�V�[���h�̓p���B�Ŕj��I\n�ړ��FL�X�e�B�b�N\n�U���FB�{�^��\n����FR�{�^��\n�p���B�FL�{�^��";
            _tutorialText.text = setText;
        }
        else
        {
            string setText = "�V�[���h�̓p���B�Ŕj��I\n�ړ��FWASD\n�U���F���N���b�N\n����FShift\n�p���B�FE";
            _tutorialText.text = setText;
        }
    }
}
