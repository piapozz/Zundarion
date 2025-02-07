using System.Collections;
using UnityEngine;
using UnityEditor.Animations;

public class PlayerMove : BaseAction
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>プレイヤーのアニメーター</summary>
    private Animator _animator = null;

    /// <summary>プレイヤーの親コンボーネント</summary>
    private BasePlayer _player = null;

    /// <summary>アニメーションパラメーターの情報</summary>
    private PlayerAnimation _animationPram = null;

    /// <summary>アニメーションパラメーターの情報</summary>
    private TagData _collisionPram;

    /// <summary>回避の再生時間</summary>
    private float _avoidanceTime = 0f;

    private System.Action<Vector3, float> _Move = null;

    public void SetCallback(System.Action<Vector3, float> action)
    {
        _Move = action;
    }

    public void Initialize()
    {

    }

    public void Execute()
    {
        /*
        // 0 レイヤーの再生されているアニメーション情報を呼び出す
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // 回避になったら走りに遷移させる
        if (stateInfo.IsName(_animationPram.movePram[(int)PlayerAnimation.MoveAnimation.AVOIDANCE]) &&
            _player.selfMoveState != PlayerAnimation.MoveAnimation.RUN)
            _player.selfMoveState = PlayerAnimation.MoveAnimation.RUN;
        */
    }

    /*
    /// <summary>
    /// 入力に応じて角度の変更とアニメーションの再生をする
    /// </summary>
    public void Move(Vector2 moveVec, PlayerAnimation.MoveAnimation moveState)
    {
        // 移動できるアニメーション状況なら
        if (!CheckAssailable()) return;

        // 現在の移動ステートが変わっていたならフラグを切り替える
        if (CheckChangeMoveState(moveState)) ChangeMoveState(moveState);

        // カメラの方向に基づいて入力ベクトルを修正
        Vector3 cameraForward = CameraManager.instance.selfCamera.transform.forward;
        Vector3 cameraRight = CameraManager.instance.selfCamera.transform.right;

        // カメラ方向に基づいた移動ベクトルを計算
        Vector3 adjustedMove = (cameraRight * moveVec.x + cameraForward * moveVec.y).normalized;

        // 向きを計算して更新
        float angle = Mathf.Atan2(adjustedMove.z, adjustedMove.x) * Mathf.Rad2Deg;

        _player.selfFrontAngleZ = angle;
    }

    /// <summary>
    /// 現在のアニメーションが移動可能な状態なのか調べる
    /// </summary>
    bool CheckAssailable()
    {
        // 結果
        bool result = false;

        // 0 レイヤーの再生されているアニメーション情報を呼び出す
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // 現在のアニメーションがIdle,Move,Run,Avoidならtrue
        result = stateInfo.IsName(_animationPram.movePram[(int)PlayerAnimation.MoveAnimation.IDLE]) ||
            stateInfo.IsName(_animationPram.movePram[(int)PlayerAnimation.MoveAnimation.WALK]) ||
            stateInfo.IsName(_animationPram.movePram[(int)PlayerAnimation.MoveAnimation.AVOIDANCE]) ||
            stateInfo.IsName(_animationPram.movePram[(int)PlayerAnimation.MoveAnimation.RUN]);

        return result;
    }

    /// <summary>
    /// 現在の移動ステートが変わっているか調べる
    /// trueなら変わっている
    /// </summary>
    bool CheckChangeMoveState(PlayerAnimation.MoveAnimation state)
    {
        // 結果
        bool result = true;

        // 0 レイヤーの再生されているアニメーション情報を呼び出す
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // 現在のステートとアニメーションが違っていたら
        result = _animator.GetInteger("Move") != (int)state;

        return result;
    }

    /// <summary>
    /// アニメーションIndexに切り替えとそれに伴う処理をする
    /// </summary>
    void ChangeMoveState(PlayerAnimation.MoveAnimation state)
    {
        // MoveIndexを変更
        switch (state)
        {
            case PlayerAnimation.MoveAnimation.IDLE:
            case PlayerAnimation.MoveAnimation.WALK:
            case PlayerAnimation.MoveAnimation.RUN:
                {
                    _animator.SetInteger("Move", (int)state);
                    break;
                }


            case PlayerAnimation.MoveAnimation.AVOIDANCE:
                {
                    _animator.SetInteger("Move", (int)state);
                    break;
                }

        }
    }
    */
}
