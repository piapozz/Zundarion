using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class OccurrenceCollision : StateMachineBehaviour
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>プレイヤーの移動データ</summary>
    [SerializeField]
    private CollisionAction _collisionAction = null;

    /// <summary>プレイヤーの当たり判定発生情報</summary>
    [SerializeField]
    private float _occurrenceTime;

    /// <summary>ダメージ量</summary>
    [SerializeField]
    private float _damage;

    /// <summary>発生させるレイヤー</summary>
    [SerializeField]
    private CollisionAction.CollisionLayer layer;

    /// <summary>発生させるタグ</summary>
    [SerializeField]
    private CollisionAction.CollisionTag tag;

    /// <summary>状態内で一度だけ生成するためのフラグ</summary>
    private bool _hasGeneratedCollision = false;

    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 特定のタイミングが来ていて、かつまだ生成していない場合のみ実行
        if (!_hasGeneratedCollision && stateInfo.normalizedTime > _occurrenceTime)
        {
            // 当たり判定を生成する
            CreateCollision.AttackData data = new CreateCollision.AttackData().zero();

            data.position = animator.transform.position;
            data.radius = 2;
            data.layer = _collisionAction.collisionLayers[(int)layer];
            data.tagname = _collisionAction.collisionTags[(int)tag];
            data.time = 2;
            data.damage = _damage;

            // 生成
            CreateCollision.instance.CreateCollisionSphere(animator.gameObject, data);

            // 一度だけ実行するためフラグを立てる
            _hasGeneratedCollision = true;
        }
       
    }
    public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 状態開始時にフラグをリセット
        _hasGeneratedCollision = false;
    }
}
