using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerCollision : MonoBehaviour
{
    [SerializeField] CollisionAction collisionAction;
    bool canParry;

    public bool GetCanParry()
    {
        return canParry;
    }

    void OnTriggerStay(Collider other)
    {
        string hitLayerName = LayerMask.LayerToName(other.gameObject.layer);
        string hitTagName = other.gameObject.tag;

        // �ڐG����Collision���G�̍U���Ȃ�canParry��؂�ւ�
        if (hitLayerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.ENEMY_ATTACK] &&
            hitTagName == "AttackNormal")
        {
            canParry = true;
            Debug.Log("�p���B�\");
        }
        else
            canParry = false;
    }
}
