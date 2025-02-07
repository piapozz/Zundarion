/*
* @file PreInput.cs
* @brief ��s���͂��L�^
* @author ishihara
* @date 2025/2/7
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PreInput : MonoBehaviour
{
    /// ��s���͂��L�^���郊�X�g
    private List<float> _preInputList = new List<float>((int)InputType.Max);

    /// <summary>
    /// ���͋L�^���m�F���� 
    /// </summary>
    /// <param name="inputType"></param>
    /// <param name="sec"></param>
    /// <returns></returns>
    public bool IsPreInput(InputType inputType, float sec)
    {
        return (Time.time - _preInputList[(int)inputType]) < sec;
    }

    /// <summary>
    /// Actions��Move�Ɋ��蓖�Ă��Ă�����͂��������Ȃ���s
    /// </summary>
    /// <param name="context"></param>
    public void RecordMove(InputAction.CallbackContext context)
    {
        _preInputList[(int)InputType.Move] = Time.time;
    }

    /// <summary>
    /// Actions��Run�Ɋ��蓖�Ă��Ă�����͂��������Ȃ���s
    /// </summary>
    /// <param name="context"></param>
    public void RecordRun(InputAction.CallbackContext context)
    {
        _preInputList[(int)InputType.Run] = Time.time;
    }

    /// <summary>
    /// Actions��Attak�Ɋ��蓖�Ă��Ă�����͂��������Ȃ���s
    /// </summary>
    /// <param name="context"></param>
    public void RecordAttack(InputAction.CallbackContext context)
    {
        _preInputList[(int)InputType.Attack] = Time.time;
    }

    /// <summary>
    /// Actions��Parry�Ɋ��蓖�Ă��Ă�����͂��������Ȃ���s
    /// </summary>
    /// <param name="context"></param>
    public void RecordParry(InputAction.CallbackContext context)
    {
        _preInputList[(int)InputType.Parry] = Time.time;
    }
}
