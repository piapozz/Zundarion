using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleProcessor : MonoBehaviour
{
    [SerializeField]
    private int _battleNum = -1;

    // ���E�F�[�u���邩
    private int _waveNum = -1;
    // �����E�F�[�u�ڂ�
    private int _waveCount = -1;
    // �����̂�
    private int _remainEnemy = -1;

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        StageManager.instance.StartBattle(_battleNum);
    }
}
