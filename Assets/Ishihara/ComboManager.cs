using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _comboText = null;    // �R���{����\������e�L�X�g
    [SerializeField]
    private float _comboTime = 1.5f;              // �R���{���r�؂��܂ł̎���
    
    private float _comboTimer = 0;                // �R���{�^�C�}�[
    private int _comboCount = 0;                  // �R���{��


}
