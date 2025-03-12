using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SystemObject
{
    public static bool isGameOver { get; private set; } = false;

    public override void Initialize()
    {
        isGameOver = false;
    }

    public static void SetGameOver()
    {
        isGameOver = true;
    }
}
