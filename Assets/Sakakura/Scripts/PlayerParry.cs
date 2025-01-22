using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    /// <summary>プレイヤーの親コンボーネント</summary>
    private BasePlayer _player = null;

    /// <summary>プレイヤーのアニメーター</summary>
    private Animator _animator = null;

    /// <summary>プレイヤーの当たり判定チェック</summary>
    private CheckCollision _checkCollision = null;

    private void Start()
    {
        _player = GetComponent<BasePlayer>();           // プレイヤー取得
        _animator = _player.selfAnimator;               // アニメーター取得
        _checkCollision = _player.selfCheckCollision;
    }

    public void Parry()
    {
        // パリィになるか判定
        if (_checkCollision.parryList.Count == 0) return;
        Debug.Log(_checkCollision.parryList.Count);
        // アニメーションをセット
        _animator.SetTrigger("Parry");
        // パリィ相手のアニメーションをひるみにする
        _checkCollision.parryList[0].selfAnimator.SetTrigger("Impact");
        // プレイヤーを敵の方向に向ける
        _player.TurnAround(_checkCollision.parryList[0].transform);
        // 通常カメラをリセット
        CameraManager.instance.SetFreeCam(transform.eulerAngles.y, 0.5f);
    }
}
