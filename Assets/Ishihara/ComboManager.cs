using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboManager : SystemObject
{
    [SerializeField] 
    private TextMeshProUGUI _comboText = null;    // コンボ数を表示するテキスト
    [SerializeField]
    private Canvas _comboCanvas = null;
    [SerializeField]
    private float COMBO_TIME_RESET = 1.5f;              // コンボが途切れるまでの時間

    public static ComboManager instance = null;

    private float _comboTime = 0;                // コンボタイマー
    private int _comboCount = 0;                 // コンボ数
    public static int comboMaximum = 0;
    private FontEffect _comboEffect = null;
    private UniTask task;

    public override void Initialize()
    {
        instance = this;
        _comboCanvas.worldCamera = Camera.main;
        comboMaximum = 0;
        _comboText.text = " ";
        _comboEffect = _comboText.gameObject.GetComponent<FontEffect>();
        if (task.Status.IsCompleted()) task = ComboTimer();
    }

    public void AddCombo()
    {
        _comboCount++;
        _comboText.text = _comboCount.ToString();
        float size = Mathf.Min((_comboCount * 0.05f) + 1 , 2.5f);
        _comboEffect.FadeIn(size);
        _comboTime = 0;

        if(_comboCount >= comboMaximum)comboMaximum = _comboCount;
    }

    private async UniTask ComboTimer()
    {
        while (_comboText != null)
        {
            if (_comboCount == 0)
            {
                _comboText.text = " ";
                _comboText.gameObject.SetActive(false);
                await UniTask.DelayFrame(1);
                continue;
            }
            else
            {
                _comboText.gameObject.SetActive(true);
            }

            _comboTime += Time.deltaTime;

            if(_comboTime > COMBO_TIME_RESET)
            {
                await BreakCombo();
            }

            if(_comboCount < 10) _comboText.color = Color.gray;
            else if(_comboCount < 20) _comboText.color = Color.red;
            else if(_comboCount < 30) _comboText.color = Color.blue;
            else _comboText.color = Color.green;

            await UniTask.DelayFrame(1);
        }
    }

    public async UniTask BreakCombo()
    {
        _comboCount = 0;
    } 
}
