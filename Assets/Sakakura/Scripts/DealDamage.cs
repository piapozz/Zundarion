using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    CollisionAction collisionAction;

    // 通り抜けたときに１度だけ呼ばれる
    void OnTrrigerEnter(Collider other)
    {
        string thisTag = transform.parent.tag;
        string hitLayerName = LayerMask.LayerToName(other.gameObject.layer);
        string hitTagName = other.gameObject.tag;
        //プレイヤーの攻撃が敵に当たったら
        if ((hitTagName == "Enemy" && thisTag == "Player") || 
            (hitTagName == "Player" && thisTag == "Enemy"))
        {
            // ダメージを与える
            other.GetComponent<BasePlayer>().TakeDamage(5);
        }
    }
}
