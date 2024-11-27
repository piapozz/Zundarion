using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerCollision : MonoBehaviour
{
    CollisionAction collisionAction;
    bool canParry;

    public bool GetCanParry()
    {
        return canParry;
    }


    void OnTriggerStay(Collider other)
    {
        string hitLayerName = LayerMask.LayerToName(other.gameObject.layer);
        string hitTagName = other.gameObject.tag;
        Debug.Log("���C���[:" + hitLayerName + "�^�O:" + hitTagName);

        // �ڐG����Collision���G�̍U���Ȃ�canParry��؂�ւ�
        if (hitLayerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.ENEMY_ATTACK] &&
            hitTagName == "Enemy")
            canParry = true;
        else
            canParry = false;
    }
}
