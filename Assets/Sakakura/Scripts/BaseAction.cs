using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseAction
{
    /// <summary>
    /// ����������
    /// </summary>
    public void Initialize();

    /// <summary>
    /// ���s����
    /// </summary>
    public void Execute();
}
