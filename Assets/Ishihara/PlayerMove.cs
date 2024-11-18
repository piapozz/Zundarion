using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEditor.Animations;

public class PlayerMove : MonoBehaviour
{
    // public //////////////////////////////////////////////////////////////////

    public enum MoveState
    {
        IDLE,
        WALK,
        AVOIDANCE,
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

    /// <summary>回避の再生時間</summary>
    private float _avoidanceTime = 0f;


    // Start is called before the first frame update
    /// <summary>
    /// 開始時に１度呼ばれる
    /// </summary>
    void Start()
    {
        _animator = GetComponent<Animator>();       // アニメーター取得
        _palyer = GetComponent<BasePlayer>();       // プレイヤー取得
        _animationPram = _palyer.selfAnimationData; // アニメーションデータ取得

        AnimatorController controller = _animator.runtimeAnimatorController as AnimatorController;
        if (controller == null) return;

        foreach (var layer in controller.layers)
        {
            foreach (var state in layer.stateMachine.states)
            {
                if (state.state.name == _animationPram.movePram[(int)MoveState.AVOIDANCE])
                {
                    Motion motion = state.state.motion;
                    if (motion is AnimationClip clip)
                    {
                        _avoidanceTime = clip.length;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    /// <summary>
    /// ゲームループで（1秒間に何回も）呼ばれる
    /// </summary>
    void FixedUpdate()
    {
        // 0 レイヤーの再生されているアニメーション情報を呼び出す
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // 回避になったら走りに遷移させる
        if (stateInfo.IsName(_animationPram.movePram[(int)MoveState.AVOIDANCE]) &&
            _palyer.selfMoveState !=　MoveState.RUN)
            _palyer.selfMoveState = MoveState.RUN;
           
    }

    /// <summary>
    /// 入力に応じて角度の変更とアニメーションの再生をする
    /// </summary>
    public void Move(Vector3 moveVec, MoveState moveState)
    {
        // 移動できるアニメーション状況なら
        if (!CheckAssailable()) return;

        // 現在の移動ステートが変わっていたならフラグを切り替える
        if (CheckChangeMoveState(moveState)) ChangeMoveState(moveState);

        // 向きを入力の方向に向く
        float temp;

        temp = Mathf.Atan2(moveVec.z, moveVec.x);

        _palyer.selfFrontAngleZ = temp * Mathf.Rad2Deg;
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
        result = stateInfo.IsName(_animationPram.movePram[(int)MoveState.IDLE]) || 
            stateInfo.IsName(_animationPram.movePram[(int)MoveState.WALK]) || 
            stateInfo.IsName(_animationPram.movePram[(int)MoveState.AVOIDANCE]) || 
            stateInfo.IsName(_animationPram.movePram[(int)MoveState.RUN]);

        return result;
    }

    /// <summary>
    /// 現在の移動ステートが変わっているか調べる
    /// trueなら変わっている
    /// </summary>
    bool CheckChangeMoveState(MoveState state)
    {
        // 結果
        bool result = false;

        // 0 レイヤーの再生されているアニメーション情報を呼び出す
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // 現在のステートとアニメーションが違っていたら
        result = stateInfo.IsName(_animationPram.movePram[(int)state]);

        return result;
    }

    /// <summary>
    /// アニメーションIndexに切り替えとそれに伴う処理をする
    /// </summary>
    void ChangeMoveState(MoveState state)
    {
        // MoveIndexを変更
        switch (state)
        {
            case MoveState.IDLE:
            case MoveState.WALK:
            case MoveState.RUN:
            {
                _animator.SetInteger("Move", (int)state);
                break;
            }


            case MoveState.AVOIDANCE:
            {
                _animator.SetInteger("Move", (int)state);

                    // 回避当たり判定を生成

                    // パラメーターを準備
                    CreateCollision.AttackData data = new CreateCollision.AttackData().zero();

                    data.position = _palyer.transform.position;
                    data.radius = 2;
                    data.layer = "Player";
                    data.tagname = "Avoidance";
                    data.time = _avoidanceTime;

                    // 生成
                    CreateCollision.instance.CreateCollisionSphere(_palyer.gameObject, data);

                break;
            }

        }
    }
}
