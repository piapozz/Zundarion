using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CollisionAction")]
public class CollisionAction : ScriptableObject
{
    public enum CollisionTag
    {
        ATTACK_NORMAL,       // �U��
        ATTACK_PASSABLE,    // �p���B�\�\��
        ATTACK_DANGEROUS,   // �p���B�s�\��
        AVOIDANCE,          // ���

        MAX
    }

    public enum CollisionLayer
    {
        PLAYER_ATTACK,      // �v���C���[�U��
        PLAYER_SURVIVE,     // �v���C���[�󂯎�
        ENEMY_ATTACK,       // �G�l�~�[�U��
        ENEMY_SURVIVE,      // �G�l�~�[�󂯎�

        MAX
    }

    public string[] collisionTags;       // �����蔻��̃^�O
    public string[] collisionLayers;     // �����蔻��̃��C���[
}
