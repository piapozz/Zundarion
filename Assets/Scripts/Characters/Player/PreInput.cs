/*
* @file PreInput.cs
* @brief 先行入力を記録
* @author ishihara
* @date 2025/2/7
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PreInput : MonoBehaviour
{
    /// 先行入力を記録するリスト
    private List<float> _preInputList = new List<float>((int)InputType.Max);

    /// <summary>
    /// 入力記録を確認する 
    /// </summary>
    /// <param name="inputType"></param>
    /// <param name="sec"></param>
    /// <returns></returns>
    public bool IsPreInput(InputType inputType, float sec)
    {
        return (Time.time - _preInputList[(int)inputType]) < sec;
    }

    /// <summary>
    /// ActionsのMoveに割り当てられている入力があったなら実行
    /// </summary>
    /// <param name="context"></param>
    public void RecordMove(InputAction.CallbackContext context)
    {
        _preInputList[(int)InputType.Move] = Time.time;
    }

    /// <summary>
    /// ActionsのRunに割り当てられている入力があったなら実行
    /// </summary>
    /// <param name="context"></param>
    public void RecordRun(InputAction.CallbackContext context)
    {
        _preInputList[(int)InputType.Run] = Time.time;
    }

    /// <summary>
    /// ActionsのAttakに割り当てられている入力があったなら実行
    /// </summary>
    /// <param name="context"></param>
    public void RecordAttack(InputAction.CallbackContext context)
    {
        _preInputList[(int)InputType.Attack] = Time.time;
    }

    /// <summary>
    /// ActionsのParryに割り当てられている入力があったなら実行
    /// </summary>
    /// <param name="context"></param>
    public void RecordParry(InputAction.CallbackContext context)
    {
        _preInputList[(int)InputType.Parry] = Time.time;
    }
}
