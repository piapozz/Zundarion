using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    CollisionAction collisionAction;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        string layerName = LayerMask.LayerToName(transform.parent.gameObject.layer);
        string hitTagName = collision.transform.tag;

        // �v���C���[�̉���Ȃ�
        if (layerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.PLAYER_SURVIVE] &&
            (hitTagName == collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_PASSABLE] ||
            hitTagName == collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_DANGEROUS]))
        {
            // �W���X�g�������
        }
        // �v���C���[�̍U���Ȃ�
        else if (layerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.PLAYER_ATTACK] &&
            collision.gameObject.tag == "Enemy")
        {
            // �_���[�W��^����
            //transform.parent.Damage(collision);
        }
        // �G�̉���Ȃ�
        else if (layerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.ENEMY_SURVIVE] &&
            (hitTagName == collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_PASSABLE] ||
            hitTagName == collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ATTACK_DANGEROUS]))
        {
            // �W���X�g�������
        }
        // �G�̍U���Ȃ�
        else if (layerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.ENEMY_ATTACK] &&
            collision.gameObject.tag == "Player")
        {
            // �_���[�W��^����
            //transform.parent.Damage(collision);
        }
    }
}
