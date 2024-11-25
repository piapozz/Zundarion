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
        // 体力チェック
        CheckDeadCharacter(1);
    }

    void FixedUpdate()
    {
        // キャラチェンジのクールダウン
        if (_charaChangeCoolDown > 0)
            _charaChangeCoolDown--;
    }

    // キャラの切り替え方向と出現位置を指定してキャラを切り替える関数
    void CharaChange(int charaDir, Vector3 entryOffset)
    {
        // 次の登場位置を決める
        Vector3 playerPos = _playerList[nowCharaNum].transform.position;
        Vector3 rotate = _playerList[nowCharaNum].transform.localEulerAngles;

        // 今のキャラを消す
        _playerList[nowCharaNum].SetActive(false);

        // 今のキャラの番号を変更
        ChangeCharaNum(charaDir);

        // 新しいキャラに切り替える
        _playerList[nowCharaNum].SetActive(true);
        _playerList[nowCharaNum].transform.position = playerPos + entryOffset;
        _playerList[nowCharaNum].transform.localEulerAngles = rotate;
        AttachPlayer(nowCharaNum);
        _animator.SetTrigger("TransFrontline");
    }

    // カメラにプレイヤーの設定をする関数
    void AttachPlayer(int playerNum)
    {
        _animator = _playerList[playerNum].transform.GetComponent<Animator>();
        _stateCam.Follow = _playerList[playerNum].transform;
        _stateCam.LookAt = _playerList[playerNum].transform;
        _stateCam.m_AnimatedTarget = _animator;
    }

    // キャラが死亡しているか判定する関数
    void CheckDeadCharacter(float health)
    {
        if (health <= 0)
        {
            _playerList.RemoveAt(nowCharaNum);

            CharaChange(1, Vector3.zero);
        }
    }

    // キャラを切り替え方向に切り替える関数
    void ChangeCharaNum(int charaDir)
    {
        // 今のキャラの番号を変更
        nowCharaNum += charaDir;
        if (nowCharaNum < 0)
            nowCharaNum = _playerList.Count - 1;
        else if (nowCharaNum >= _playerList.Count)
            nowCharaNum = 0;
    }

    // パリィする関数
    void Parry(int charaDir)
    {
        // キャラチェンジ
        CharaChange(charaDir, Vector3.zero);
        _animator.SetTrigger("Parry");
        // 通常カメラをリセット
        _freeLookCam.m_XAxis.Value = _playerList[nowCharaNum].transform.eulerAngles.y;
        _freeLookCam.m_YAxis.Value = 0.5f;
    }

    // 次のキャラに切り替える関数
    public void OnChangeNextChara()
    {
        if (_charaChangeCoolDown > 0) return;

        _charaChangeCoolDown = CHARA_CHANGE_COOL_TIME;

        // パリィになるか判定
        if (_checkCollisionList[nowCharaNum].GetCanParry())
            Parry(1);
        else
            CharaChange(1, -_playerList[nowCharaNum].transform.forward * 2.0f);
    }

    // 前のキャラに切り替える関数
    public void OnChangeBeforeChara()
    {
        if (_charaChangeCoolDown > 0) return;

        _charaChangeCoolDown = CHARA_CHANGE_COOL_TIME;

        // パリィになるか判定
        if (_checkCollisionList[nowCharaNum].GetCanParry())
            Parry(-1);
        else
            CharaChange(-1, -_playerList[nowCharaNum].transform.forward * 2.0f);
    }

    // 今のアクティブなキャラを取得する関数
    public GameObject GetActiveChara()
    {
        return _playerList[nowCharaNum];
    }
}
