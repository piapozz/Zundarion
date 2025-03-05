using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    public TMP_Text text;
    public float blinkSpeed = 1f;

    void Update()
    {
        if (text != null)
        {
            Color color = text.color;
            color.a = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);
            text.color = color;
        }
    }
}
