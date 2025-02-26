using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _comboText = null;    // コンボ数を表示するテキスト
    [SerializeField]
    private float _comboTime = 1.5f;              // コンボが途切れるまでの時間
    
    private float _comboTimer = 0;                // コンボタイマー
    private int _comboCount = 0;                  // コンボ数


}
