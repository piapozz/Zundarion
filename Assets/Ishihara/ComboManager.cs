using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboManager : SystemObject
{
    [SerializeField] 
    private TextMeshProUGUI _comboText = null;    // コンボ数を表示するテキスト
    [SerializeField]
    private float _comboTime = 1.5f;              // コンボが途切れるまでの時間

    public static ComboManager instance = null;

    private float _comboTimer = 0;                // コンボタイマー
    private int _comboCount = 0;                  // コンボ数

    public override void Initialize()
    {
        instance = this;
    }
}
