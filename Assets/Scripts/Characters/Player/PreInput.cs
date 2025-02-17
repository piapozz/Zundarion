/*
* @file PreInput.cs
* @brief ��s���͂��L�^
* @author sakakura
* @date 2025/2/7
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PreInput : MonoBehaviour
{
    /// <summary>���͂̎��</summary>
    public InputType preInputType { get; private set; } = InputType.None;

    /// <summary>���͎���</summary>
    private float _preInputTime = 0.0f;

    public void Initialize()
    {

    }

    /// <summary>
    /// ���͋L�^���m�F���� 
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
    /// Actions��Move�Ɋ��蓖�Ă��Ă�����͂��������Ȃ�L�^����
    /// </summary>
    /// <param name="context"></param>
    public void RecordMove(InputAction.CallbackContext context)
    {
        RecordPreInput(InputType.Move, Time.time);
    }

    /// <summary>
    /// Actions��Run�Ɋ��蓖�Ă��Ă�����͂��������Ȃ�L�^����
    /// </summary>
    /// <param name="context"></param>
    public void RecordRun(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        RecordPreInput(InputType.Run, Time.time);
    }

    /// <summary>
    /// Actions��Attak�Ɋ��蓖�Ă��Ă�����͂��������Ȃ�L�^����
    /// </summary>
    /// <param name="context"></param>
    public void RecordAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        RecordPreInput(InputType.Attack, Time.time);
    }

    /// <summary>
    /// Actions��Parry�Ɋ��蓖�Ă��Ă�����͂��������Ȃ�L�^����
    /// </summary>
    /// <param name="context"></param>
    public void RecordParry(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        RecordPreInput(InputType.Parry, Time.time);
    }

    /// <summary>
    /// ��s���͂��L�^����
    /// </summary>
    /// <param name="setInput"></param>
    /// <param name="setTime"></param>
    private void RecordPreInput(InputType setInput, float setTime)
    {
        preInputType = setInput;
        _preInputTime = setTime;
    }

    /// <summary>
    /// ��s���͂̋L�^���N���A����
    /// </summary>
    public void ClearRecord()
    {
        preInputType = InputType.None;
        _preInputTime = 0;
    }
}
