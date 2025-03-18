using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public static bool isGameOver { get; private set; } = false;

    public static void Initialize()
    {
        isGameOver = false;
    }

    public static void SetGameOver()
    {
        isGameOver = true;
    }
}
