using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    CollisionAction collisionAction;

    // �ʂ蔲�����Ƃ��ɂP�x�����Ă΂��
    void OnTrrigerEnter(Collider other)
    {
        string thisTag = transform.parent.tag;
        string hitLayerName = LayerMask.LayerToName(other.gameObject.layer);
        string hitTagName = other.gameObject.tag;
        //�v���C���[�̍U�����G�ɓ���������
        if ((hitTagName == "Enemy" && thisTag == "Player") || 
            (hitTagName == "Player" && thisTag == "Enemy"))
        {
            // �_���[�W��^����
            other.GetComponent<BasePlayer>().TakeDamage(5);
        }
    }
}
