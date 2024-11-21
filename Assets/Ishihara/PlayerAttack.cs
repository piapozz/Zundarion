using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>プレイヤーのアニメーター</summary>
    private Animator _animator = null;

    /// <summary>プレイヤーの親コンボーネント</summary>
    private BasePlayer _player = null;

    /// <summary>当たり判定生成コンポーネント</summary>
    private CreateCollision _createCollision = null;

    /// <summary>アニメーションパラメーターの情報</summary>
    private PlayerAnimation _animationPram = null;

    /// <summary>アニメーションパラメーターの情報</summary>
    private CollisionAction _collisionPram;

    /// <summary>現在のコンボ数</summary>
    private int _comboCount = 0;

    // Start is called before the first frame update
    /// <summary>
    /// 開始時に１度呼ばれる
    /// </summary>
    void Start()
    {
        _player = GetComponent<BasePlayer>();   // プレイヤー取得
        _animator = _player.selfAnimator;   // アニメーター取得
        _animationPram = _player.selfAnimationData; // アニメーションデータ取得
        _collisionPram = _player.selfCollisionData; // コリジョンデータ取得
    }

    // Update is called once per frame
    /// <summary>
    /// ゲームループで（1秒間に何回も）呼ばれる
    /// </summary>
    void Update()
    {
        // 定期的にコンボ回数を初期化する
        //_comboCount = 0;
    }

    /// <summary>
    /// アニメーションを攻撃に更新して、当たり判定を生成する
    /// </summary>
    public void Attack()
    {
        // コンボの派生がまだあるなら
        if(_comboCount > _player.selfComboCount) return;

        // 攻撃できるアニメーション状況なら
        if(!CheckAssailable()) return;

        // カウンターを増やして。トリガーをセットする
        _comboCount++;
        _animator.SetTrigger(_animationPram.attackPram[(int)PlayerAnimation.AttackAnimation.ATTACK]);

        // 当たり判定を生成する
        // パラメーターを準備
        CreateCollision.AttackData data = new CreateCollision.AttackData().zero();

        data.position = _player.transform.position;
        data.radius = 2;
        data.layer = _collisionPram.collisionLayers[(int)CollisionAction.CollisionLayer.PLAYER_ATTACK];
        data.tagname = _collisionPram.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_NOMAL];
        data.time = 2;

        // 生成
        CreateCollision.instance.CreateCollisionSphere(_player.selfGameObject, data);
    }

    /// <summary>
    /// 現在のアニメーションが攻撃可能な状態なのか調べる
    /// </summary>
    bool CheckAssailable()
    {
        bool result;

        // 0 レイヤーの再生されているアニメーション情報を呼び出す
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // 現在のステートとアニメーションが違っていたら
        result = !stateInfo.IsName("Attack_1") &&
                !stateInfo.IsName("Attack_2") &&
                !stateInfo.IsName("Attack_3") &&
                !_animator.IsInTransition(0);

        

        return result;
    }

}
