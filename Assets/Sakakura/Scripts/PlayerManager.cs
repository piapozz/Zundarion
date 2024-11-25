using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private int nowCharaNum = 0;
    private const int CHARA_CHANGE_COOL_TIME = 60;

    [SerializeField] private List<GameObject> _playerList;
    [SerializeField] private CinemachineStateDrivenCamera _stateCam;
    private CinemachineFreeLook _freeLookCam;
    private Animator _animator;
    private List<CheckCollision> _checkCollisionList = new List<CheckCollision>();
    private int _charaChangeCoolDown;

    void Start()
    {
        for (int i = 0; i < _playerList.Count; i++)
        {
            CheckCollision cc = _playerList[i].GetComponent<CheckCollision>();
            _checkCollisionList.Add(cc);

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

    void FixedUpdate()
    {
        // �L�����`�F���W�̃N�[���_�E��
        if (_charaChangeCoolDown > 0)
            _charaChangeCoolDown--;
    }

    // �L�����̐؂�ւ������Əo���ʒu���w�肵�ăL������؂�ւ���֐�
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

    // �J�����Ƀv���C���[�̐ݒ������֐�
    void AttachPlayer(int playerNum)
    {
        _animator = _playerList[playerNum].transform.GetComponent<Animator>();
        _stateCam.Follow = _playerList[playerNum].transform;
        _stateCam.LookAt = _playerList[playerNum].transform;
        _stateCam.m_AnimatedTarget = _animator;
    }

    // �L���������S���Ă��邩���肷��֐�
    void CheckDeadCharacter(float health)
    {
        if (health <= 0)
        {
            _playerList.RemoveAt(nowCharaNum);

            CharaChange(1, Vector3.zero);
        }
    }

    // �L������؂�ւ������ɐ؂�ւ���֐�
    void ChangeCharaNum(int charaDir)
    {
        // ���̃L�����̔ԍ���ύX
        nowCharaNum += charaDir;
        if (nowCharaNum < 0)
            nowCharaNum = _playerList.Count - 1;
        else if (nowCharaNum >= _playerList.Count)
            nowCharaNum = 0;
    }

    // �p���B����֐�
    void Parry(int charaDir)
    {
        // �L�����`�F���W
        CharaChange(charaDir, Vector3.zero);
        _animator.SetTrigger("Parry");
        // �ʏ�J���������Z�b�g
        _freeLookCam.m_XAxis.Value = _playerList[nowCharaNum].transform.eulerAngles.y;
        _freeLookCam.m_YAxis.Value = 0.5f;
    }

    // ���̃L�����ɐ؂�ւ���֐�
    public void OnChangeNextChara()
    {
        if (_charaChangeCoolDown > 0) return;

        _charaChangeCoolDown = CHARA_CHANGE_COOL_TIME;

        // �p���B�ɂȂ邩����
        if (_checkCollisionList[nowCharaNum].GetCanParry())
            Parry(1);
        else
            CharaChange(1, -_playerList[nowCharaNum].transform.forward * 2.0f);
    }

    // �O�̃L�����ɐ؂�ւ���֐�
    public void OnChangeBeforeChara()
    {
        if (_charaChangeCoolDown > 0) return;

        _charaChangeCoolDown = CHARA_CHANGE_COOL_TIME;

        // �p���B�ɂȂ邩����
        if (_checkCollisionList[nowCharaNum].GetCanParry())
            Parry(-1);
        else
            CharaChange(-1, -_playerList[nowCharaNum].transform.forward * 2.0f);
    }

    // ���̃A�N�e�B�u�ȃL�������擾����֐�
    public GameObject GetActiveChara()
    {
        return _playerList[nowCharaNum];
    }
}
