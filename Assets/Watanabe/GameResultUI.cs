using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameResultUI : MonoBehaviour
{
    public TMP_Text resultText;
    public TMP_Text comboMaximumUI;


    void Start()
    {
        if(BasePlayer.isPlayerDead == true)
        {
            resultText.text = string.Format("GameOver");
            resultText.color = Color.red;
        }
        else 
        {
            resultText.text = string.Format("GameClear"); 
            resultText.color = Color.green;
        }

        comboMaximumUI.text = string.Format("{0}", ComboManager.comboMaximum);
    }

}
