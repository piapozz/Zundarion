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

        // ÚG‚µ‚½Collision‚ª“G‚ÌUŒ‚‚È‚çcanParry‚ğØ‚è‘Ö‚¦
        if (hitLayerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.ENEMY_ATTACK] &&
            hitTagName == "AttackNormal")
        {
            canParry = true;
            Debug.Log("ƒpƒŠƒB‰Â”\");
        }
        else
            canParry = false;
    }
}
