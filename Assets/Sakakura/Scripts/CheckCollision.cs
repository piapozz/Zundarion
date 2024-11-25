using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    CollisionAction collisionAction;
    private bool canParry;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public bool GetCanParry()
    {
        return canParry;
    }

    private void OnCollisionEnter(Collision collision)
    {
        string layerName = LayerMask.LayerToName(transform.parent.gameObject.layer);
        string hitTagName = collision.transform.tag;

        // プレイヤーの回避なら
        if (layerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.PLAYER_SURVIVE] &&
            (hitTagName == collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_PASSABLE] ||
            hitTagName == collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_DANGEROUS]))
            // パリィ可能
            canParry = true;
        else
            canParry = false;

        // プレイヤーの攻撃なら
        if (layerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.PLAYER_ATTACK] &&
            collision.gameObject.tag == "Enemy")
        {
            // ダメージを与える
            //transform.parent.Damage(collision);
        }

        // 敵の回避なら
        if (layerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.ENEMY_SURVIVE] &&
            (hitTagName == collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_PASSABLE] ||
            hitTagName == collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_DANGEROUS]))
            canParry = true;
        else
            canParry = false;

        // 敵の攻撃なら
        if (layerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.ENEMY_ATTACK] &&
            collision.gameObject.tag == "Player")
        {
            // ダメージを与える
            //transform.parent.Damage(collision);
        }
    }
}
