using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int nowCharaNum = 0;

    [SerializeField] private GameObject[] _playerModel;
    [SerializeField] CinemachineStateDrivenCamera _stateCam;
    [SerializeField] private CapsuleCollider _capsuleCollider;

    private Animator _animator;

    void Start()
    {
        for (int i = 0; i < _playerModel.Length; i++)
        {
            if (i == nowCharaNum)
            {
                _playerModel[i].gameObject.SetActive(true);
                AttachPlayer(i);
            }
            else
                _playerModel[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // キャラチェンジ
            CharaChange(-1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // キャラチェンジ
            CharaChange(1);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            _animator.SetTrigger("Parry");
            // 通常カメラをリセット
            
        }
    }

    void CharaChange(int charaDir)
    {
        // 今のキャラを消す
        _playerModel[nowCharaNum].SetActive(false);

        // 今のキャラの番号を変更
        nowCharaNum += charaDir;
        if (nowCharaNum < 0)
            nowCharaNum = _playerModel.Length - 1;
        else if (nowCharaNum >= _playerModel.Length)
            nowCharaNum = 0;

        // 新しいキャラに切り替える
        _playerModel[nowCharaNum].SetActive(true);
        AttachPlayer(nowCharaNum);
    }

    void AttachPlayer(int playerNum)
    {
        _animator = _playerModel[nowCharaNum].transform.GetComponent<Animator>();
        _stateCam.Follow = _playerModel[playerNum].transform;
        _stateCam.LookAt = _playerModel[playerNum].transform;
        _stateCam.m_AnimatedTarget = _animator;
        //_capsuleCollider.transform.parent = _playerModel[nowCharaNum].transform;
    }
}
