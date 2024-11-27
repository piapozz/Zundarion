using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    // �ʂ蔲�����Ƃ��ɂP�x�����Ă΂��
    void OnTriggerEnter(Collider other)
    {
        string thisTag = gameObject.transform.parent.tag;
        string hitLayerName = LayerMask.LayerToName(other.gameObject.layer);
        string hitTagName = other.gameObject.tag;
        Debug.Log("���C���[:" + hitLayerName + "�^�O:" + hitTagName);

        // �G�̍U�����v���C���[�ɓ���������
        if (hitTagName == "Player" && thisTag == "Enemy")
        {
            // �_���[�W��^����
            other.GetComponent<BasePlayer>().TakeDamage(5);
            Debug.Log("�^�_���[�W");
        }
        // �v���C���[�̍U�����G�ɓ���������
        else if (hitTagName == "Enemy" && thisTag == "Player")
        {
            other.GetComponent<EnemyBase>().ReceiveDamage(5);
            Debug.Log("�^�_���[�W");
        }
    }
}
