/*
* @file PreInput.cs
* @brief 先行入力を記録
* @author sakakura
* @date 2025/2/7
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PreInput : MonoBehaviour
{
    /// <summary>入力の種類</summary>
    public InputType preInputType { get; private set; } = InputType.None;

    /// <summary>入力時間</summary>
    private float _preInputTime = 0.0f;

    public void Initialize()
    {

    }

    /// <summary>
    /// 入力記録を確認する 
    /// </summary>
    /// <param name="inputType"></param>
    /// <param name="sec"></param>
    /// <returns></returns>
    public bool IsPreInput(InputType inputType, float sec)
    {
        if (inputType != preInputType) return false;

        return (Time.time - _preInputTime) < sec;
    }

    /// <summary>
    /// ActionsのMoveに割り当てられている入力があったなら記録する
    /// </summary>
    /// <param name="context"></param>
    public void RecordMove(InputAction.CallbackContext context)
    {
        RecordPreInput(InputType.Move, Time.time);
    }

    /// <summary>
    /// ActionsのRunに割り当てられている入力があったなら記録する
    /// </summary>
    /// <param name="context"></param>
    public void RecordRun(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        RecordPreInput(InputType.Run, Time.time);
    }

    /// <summary>
    /// ActionsのAttakに割り当てられている入力があったなら記録する
    /// </summary>
    /// <param name="context"></param>
    public void RecordAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        RecordPreInput(InputType.Attack, Time.time);
    }

    /// <summary>
    /// ActionsのParryに割り当てられている入力があったなら記録する
    /// </summary>
    /// <param name="context"></param>
    public void RecordParry(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        RecordPreInput(InputType.Parry, Time.time);
    }

    /// <summary>
    /// 先行入力を記録する
    /// </summary>
    /// <param name="setInput"></param>
    /// <param name="setTime"></param>
    private void RecordPreInput(InputType setInput, float setTime)
    {
        preInputType = setInput;
        _preInputTime = setTime;
    }

    /// <summary>
    /// 先行入力の記録をクリアする
    /// </summary>
    public void ClearRecord()
    {
        preInputType = InputType.None;
        _preInputTime = 0;
    }
}
