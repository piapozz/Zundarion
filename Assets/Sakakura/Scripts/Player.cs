using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int nowCharaNum = 0;

    [SerializeField] private GameObject[] _playerModel;
    [SerializeField] CinemachineStateDrivenCamera _stateCam;
    CinemachineFreeLook _freeLookCam;

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

        _freeLookCam = _stateCam.GetComponentInChildren<CinemachineFreeLook>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // キャラチェンジ
            CharaChange(-1, -_playerModel[nowCharaNum].transform.forward * 2.0f);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // キャラチェンジ
            CharaChange(1, -_playerModel[nowCharaNum].transform.forward * 2.0f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // キャラチェンジ
            CharaChange(1, Vector3.zero);
            _animator.SetTrigger("Parry");
            // 通常カメラをリセット
            _freeLookCam.m_XAxis.Value = _playerModel[nowCharaNum].transform.eulerAngles.y;
            _freeLookCam.m_YAxis.Value = 0.5f;
        }
    }

    void CharaChange(int charaDir, Vector3 entryOffset)
    {
        // 次の登場位置を決める
        Vector3 playerPos = _playerModel[nowCharaNum].transform.position;
        Vector3 rotate = _playerModel[nowCharaNum].transform.localEulerAngles;

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
        _playerModel[nowCharaNum].transform.position = playerPos + entryOffset;
        _playerModel[nowCharaNum].transform.localEulerAngles = rotate;
        AttachPlayer(nowCharaNum);
        _animator.SetTrigger("TransFrontline");
    }

    void AttachPlayer(int playerNum)
    {
        _animator = _playerModel[nowCharaNum].transform.GetComponent<Animator>();
        _stateCam.Follow = _playerModel[playerNum].transform;
        _stateCam.LookAt = _playerModel[playerNum].transform;
        _stateCam.m_AnimatedTarget = _animator;
    }
}
