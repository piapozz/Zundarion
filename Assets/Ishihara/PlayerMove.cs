using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // public //////////////////////////////////////////////////////////////////

    public enum MoveState
    {
        WALK,
        RUN,

        MAX
    }

    // private //////////////////////////////////////////////////////////////////

    /// <summary>プレイヤーのアニメーター</summary>
    private Animator _animator = null;

    /// <summary>プレイヤーの親コンボーネント</summary>
    private BasePlayer _palyer = null;

    /// <summary>アニメーションパラメーターの情報</summary>
    private PlayerAnimation _animationPram;


    // Start is called before the first frame update
    /// <summary>
    /// 開始時に１度呼ばれる
    /// </summary>
    void Start()
    {
        _animator = GetComponent<Animator>();       // アニメーター取得
        _palyer = GetComponent<BasePlayer>();       // プレイヤー取得
        _animationPram = _palyer.selfAnimationData; // アニメーションデータ取得
    }

    // Update is called once per frame
    /// <summary>
    /// ゲームループで（1秒間に何回も）呼ばれる
    /// </summary>
    void Update()
    {

    }

    /// <summary>
    /// 入力に応じて角度の変更とアニメーションの再生をする
    /// </summary>
    void Move(Vector3 moveVec, MoveState moveState)
    {
        // コンボの派生がまだあるなら
        if (!(moveVec == Vector3.zero)) return;

        // 移動できるアニメーション状況なら
        if (!CheckAssailable()) return;

        // フラグを切り替える
        // _animator.SetBool(_triggers[_comboCount]);

        // 向きを入力の方向に向く
        _palyer.selfFrontAngle = this.transform.forward.x;
    }

    /// <summary>
    /// 現在のアニメーションが移動可能な状態なのか調べる
    /// </summary>
    bool CheckAssailable()
    {
        return true;
    }

}
