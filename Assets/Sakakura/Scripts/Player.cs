using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private int nowCharaNum = 0;

    [SerializeField] private List<GameObject> _playerList;
    [SerializeField] private CinemachineStateDrivenCamera _stateCam;
    private CinemachineFreeLook _freeLookCam;
    private bool _canParry;

    private Animator _animator;

    void Start()
    {
        for (int i = 0; i < _playerList.Count; i++)
        {
            if (i == nowCharaNum)
            {
                _playerList[i].gameObject.SetActive(true);
                AttachPlayer(i);
            }
            else
                _playerList[i].gameObject.SetActive(false);
        }

        _freeLookCam = _stateCam.GetComponentInChildren<CinemachineFreeLook>();
    }

    void Update()
    {
        // �̗̓`�F�b�N
        CheckDeadCharacter(1);
    }

    void CharaChange(int charaDir, Vector3 entryOffset)
    {
        // ���̓o��ʒu�����߂�
        Vector3 playerPos = _playerList[nowCharaNum].transform.position;
        Vector3 rotate = _playerList[nowCharaNum].transform.localEulerAngles;

        // ���̃L����������
        _playerList[nowCharaNum].SetActive(false);

        // ���̃L�����̔ԍ���ύX
        ChangeCharaNum(charaDir);

        // �V�����L�����ɐ؂�ւ���
        _playerList[nowCharaNum].SetActive(true);
        _playerList[nowCharaNum].transform.position = playerPos + entryOffset;
        _playerList[nowCharaNum].transform.localEulerAngles = rotate;
        AttachPlayer(nowCharaNum);
        _animator.SetTrigger("TransFrontline");
    }

    void AttachPlayer(int playerNum)
    {
        _animator = _playerList[playerNum].transform.GetComponent<Animator>();
        _stateCam.Follow = _playerList[playerNum].transform;
        _stateCam.LookAt = _playerList[playerNum].transform;
        _stateCam.m_AnimatedTarget = _animator;
    }

    void CheckDeadCharacter(float health)
    {
        if (health <= 0)
        {
            _playerList.RemoveAt(nowCharaNum);

            ChangeCharaNum(1);
        }
    }

    void ChangeCharaNum(int charaDir)
    {
        // ���̃L�����̔ԍ���ύX
        nowCharaNum += charaDir;
        if (nowCharaNum < 0)
            nowCharaNum = _playerList.Count - 1;
        else if (nowCharaNum >= _playerList.Count)
            nowCharaNum = 0;
    }

    public void OnChangeNextChara()
    {
        CharaChange(1, -_playerList[nowCharaNum].transform.forward * 2.0f);
    }

    public void OnChangeBeforeChara()
    {
        CharaChange(-1, -_playerList[nowCharaNum].transform.forward * 2.0f);
    }

    public void OnParry()
    {
        // �L�����`�F���W
        CharaChange(1, Vector3.zero);
        _animator.SetTrigger("Parry");
        // �ʏ�J���������Z�b�g
        _freeLookCam.m_XAxis.Value = _playerList[nowCharaNum].transform.eulerAngles.y;
        _freeLookCam.m_YAxis.Value = 0.5f;
    }
}
