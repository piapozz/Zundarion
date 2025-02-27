using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboManager : SystemObject
{
    [SerializeField] 
    private TextMeshProUGUI _comboText = null;    // �R���{����\������e�L�X�g
    [SerializeField]
    private Canvas _comboCanvas = null;
    [SerializeField]
    private float _comboTime = 1.5f;              // �R���{���r�؂��܂ł̎���

    public static ComboManager instance = null;

    private float _comboTimer = 0;                // �R���{�^�C�}�[
    private int _comboCount = 0;                  // �R���{��

    public override void Initialize()
    {
        instance = this;
        _comboCanvas.worldCamera = Camera.main;
        _comboText.text = "0";
    }

    public void AddCombo()
    {
        _comboCount++;
        _comboText.text = _comboCount.ToString();
    }
}
