using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class OccurrenceCollision : StateMachineBehaviour
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>プレイヤーの当たり判定情報</summary>
    [SerializeField]
    private CollisionAction _collisionPram = null;

    /// <summary>プレイヤーの当たり判定発生情報</summary>
    [SerializeField]
    private float _occurrenceTime;

    /// <summary>発生させるレイヤー</summary>
    [SerializeField]
    private CollisionAction.CollisionLayer layer;

    /// <summary>発生させるタグ</summary>
    [SerializeField]
    private CollisionAction.CollisionTag tag;

    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 現在再生されている時間が特定のタイミングが来たら。
        if (stateInfo.normalizedTime >= _occurrenceTime)
        {
            // 当たり判定を生成する
            // パラメーターを準備
            CreateCollision.AttackData data = new CreateCollision.AttackData().zero();

            data.position = animator.transform.position;
            data.radius = 2;
            data.layer = _collisionPram.collisionLayers[(int)layer];
            data.tagname = _collisionPram.collisionTags[(int)tag];
            data.time = 2;

            // 生成
            CreateCollision.instance.CreateCollisionSphere(animator.gameObject, data);
        }
    }
}
