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
        // �̗̓`�F�b�N
        CheckDeadCharacter(_basePlayer.selfCurrentHealth);
    }

    // �L���������S���Ă��邩���肷��֐�
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
