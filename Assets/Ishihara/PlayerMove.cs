using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEditor.Animations;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerMove : MonoBehaviour
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>プレイヤーのアニメーター</summary>
    private Animator _animator = null;

    /// <summary>プレイヤーの親コンボーネント</summary>
    private BasePlayer _player = null;

    /// <summary>アニメーションパラメーターの情報</summary>
    private PlayerAnimation _animationPram = null;

    /// <summary>アニメーションパラメーターの情報</summary>
    private CollisionAction _collisionPram;

    /// <summary>回避の再生時間</summary>
    private float _avoidanceTime = 0f;


    // Start is called before the first frame update
    /// <summary>
    /// 開始時に１度呼ばれる
    /// </summary>
    void Start()
    {
        _player = GetComponent<BasePlayer>();       // プレイヤー取得
        _animator = _player.selfAnimator;       // アニメーター取得
        _animationPram = _player.selfAnimationData; // アニメーションデータ取得
        _collisionPram = _player.selfCollisionData; // コリジョンデータ取得

        AnimatorController controller = _animator.runtimeAnimatorController as AnimatorController;
        if (controller == null) return;

        foreach (var layer in controller.layers)
        {
            foreach (var state in layer.stateMachine.states)
            {
                if (state.state.name == _animationPram.movePram[(int)PlayerAnimation.MoveAnimation.AVOIDANCE])
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
    void Update()
    {
        // 0 レイヤーの再生されているアニメーション情報を呼び出す
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // 回避になったら走りに遷移させる
        if (stateInfo.IsName(_animationPram.movePram[(int)PlayerAnimation.MoveAnimation.AVOIDANCE]) &&
            _player.selfMoveState != PlayerAnimation.MoveAnimation.RUN)
            _player.selfMoveState = PlayerAnimation.MoveAnimation.RUN;
           
    }

    /// <summary>
    /// 入力に応じて角度の変更とアニメーションの再生をする
    /// </summary>
    public void Move(Vector2 moveVec, PlayerAnimation.MoveAnimation moveState)
    {
        // 移動できるアニメーション状況なら
        if (!CheckAssailable()) return;

        // 現在の移動ステートが変わっていたならフラグを切り替える
        if (CheckChangeMoveState(moveState)) ChangeMoveState(moveState);

        // 向きを入力の方向に向く
        float temp;

        temp = Mathf.Atan2(moveVec.y, moveVec.x);

        _player.selfFrontAngleZ = (temp * Mathf.Rad2Deg);
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

                    // 回避当たり判定を生成

                    // パラメーターを準備
                    CreateCollision.AttackData data = new CreateCollision.AttackData().zero();

                    data.position = _player.transform.position;
                    data.radius = 2;
                    data.layer = _collisionPram.collisionLayers[(int)CollisionAction.CollisionLayer.PLAYER_SURVIVE];
                    data.tagname = _collisionPram.collisionTags[(int)CollisionAction.CollisionTag.AVOIDANCE];
                    data.time = _avoidanceTime;

                    // 生成
                    CreateCollision.instance.CreateCollisionSphere(_player.gameObject, data);

                break;
            }

        }
    }
}
