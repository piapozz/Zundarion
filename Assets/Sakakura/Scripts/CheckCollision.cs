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
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);
        string hitTagName = collision.transform.tag;
        Debug.Log("���C���[:" + layerName + "�^�O:" + hitTagName);

        // �p���B
        if (hitTagName == collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_PASSABLE])
            // �p���B�\
            canParry = true;
        else
            canParry = false;

        // �v���C���[�̉���Ȃ�
        if (layerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.PLAYER_SURVIVE] &&
            (hitTagName == collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_PASSABLE] ||
            hitTagName == collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_DANGEROUS]))
        {

        }

        // �v���C���[�̍U���Ȃ�
        if (layerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.PLAYER_ATTACK] &&
            collision.gameObject.tag == "Enemy")
        {
            // �_���[�W��^����
            transform.parent.GetComponent<BasePlayer>().TakeDamage(5);
        }

        // �G�̉���Ȃ�
        if (layerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.ENEMY_SURVIVE] &&
            (hitTagName == collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_PASSABLE] ||
            hitTagName == collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_DANGEROUS]))
        {

        }

        // �G�̍U���Ȃ�
        if (layerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.ENEMY_ATTACK] &&
            collision.gameObject.tag == "Player")
        {
            // �_���[�W��^����
            transform.parent.GetComponent<BasePlayer>().TakeDamage(5);
        }
    }
}
