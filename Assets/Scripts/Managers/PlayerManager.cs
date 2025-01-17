using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private BasePlayer _basePlayer = null;

    private void Update()
    {
        // 体力チェック
        CheckDeadCharacter(_basePlayer.selfCurrentHealth);
    }

    // キャラが死亡しているか判定する関数
    private void CheckDeadCharacter(float health)
    {
        if (health <= 0)
            _basePlayer.selfAnimator.SetTrigger("Die");
    }

    public GameObject GetActiveChara()
    {
        return _basePlayer.selfAnimator.gameObject;
    }
}
