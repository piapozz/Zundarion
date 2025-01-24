using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static CommonModule;

public class PlayerParry : MonoBehaviour
{
    /// <summary>プレイヤーの親コンボーネント</summary>
    private BasePlayer _player = null;

    /// <summary>プレイヤーのアニメーター</summary>
    private Animator _animator = null;

    /// <summary>プレイヤーの当たり判定チェック</summary>
    private CheckCollision _checkCollision = null;

    private bool _isCoolDown = false;

    public readonly float PARRY_COOL_DOWN = 0.0f;

    private void Start()
    {
        _player = GetComponent<BasePlayer>();           // プレイヤー取得
        _animator = _player.selfAnimator;               // アニメーター取得
        _checkCollision = _player.selfCheckCollision;
    }

    public void Parry()
    {
        List<BaseCharacter> parryList = CollisionManager.instance.parryList;
        // パリィになるか判定
        if (parryList.Count == 0) return;
        if (_isCoolDown == true) return;
        // クールダウン開始
        _isCoolDown = true;
        UniTask task = WaitAction(PARRY_COOL_DOWN, UpCoolDown);
        // アニメーションをセット
        _animator.SetTrigger("Parry");
        // パリィ相手のアニメーションをひるみにする
        parryList[0].selfAnimator.SetTrigger("Impact");
        // プレイヤーを敵の方向に向ける
        _player.TurnAround(parryList[0].transform);
        // 通常カメラをリセット
        CameraManager.instance.SetFreeCam(transform.eulerAngles.y, 0.5f);
    }

    public void UpCoolDown()
    {
        _isCoolDown = false;
    }
}
