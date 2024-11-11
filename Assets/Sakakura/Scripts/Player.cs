using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int nowCharaNum = 0;

    [SerializeField] private GameObject[] _playerModel;
    [SerializeField] private Camera _camera;
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
            _animator.SetBool("Attack", true);
        else
            _animator.SetBool("Attack", false);
    }

    void CharaChange(int charaDir)
    {
        _playerModel[nowCharaNum].SetActive(false);

        nowCharaNum += charaDir;
        if (nowCharaNum < 0)
            nowCharaNum = _playerModel.Length - 1;
        else if (nowCharaNum >= _playerModel.Length)
            nowCharaNum = 0;

        _playerModel[nowCharaNum].SetActive(true);
        AttachPlayer(nowCharaNum);
    }

    void AttachPlayer(int playerNum)
    {
        _camera.transform.parent = _playerModel[nowCharaNum].transform;
        _capsuleCollider.transform.parent = _playerModel[nowCharaNum].transform;
        _animator = _playerModel[nowCharaNum].transform.GetComponent<Animator>();
    }
}
