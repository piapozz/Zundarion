using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    // 通り抜けたときに１度だけ呼ばれる
    void OnTriggerEnter(Collider other)
    {
        string thisTag = transform.parent.gameObject.tag;
        string hitLayerName = LayerMask.LayerToName(other.gameObject.layer);
        string hitTagName = other.gameObject.tag;
        Debug.Log("レイヤー:" + hitLayerName + "タグ:" + hitTagName);

        // 敵の攻撃がプレイヤーに当たったら
        if (hitTagName == "Player" && thisTag == "Enemy")
        {
            // ダメージを与える
            other.GetComponent<BasePlayer>().TakeDamage(20);
            Debug.Log("与ダメージ");
        }
        // プレイヤーの攻撃が敵に当たったら
        else if (hitTagName == "Enemy" && thisTag == "Player")
        {
            other.GetComponent<EnemyBase>().ReceiveDamage(20);
            Debug.Log("与ダメージ");
        }
    }
}
