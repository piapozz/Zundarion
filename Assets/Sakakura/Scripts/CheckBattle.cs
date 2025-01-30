using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBattle : MonoBehaviour
{
    [SerializeField]
    private int _battleNum = -1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        StageManager.instance.StartBattle(_battleNum);
    }
}
