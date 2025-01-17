using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CollisionAction")]
public class CollisionAction : ScriptableObject
{
    public enum CollisionTag
    {
        PLAYER,      // �v���C���[
        ENEMY,       // �G�l�~�[

        MAX
    }

    public enum CollisionLayer
    {
        ATTACK,      // �U��
        ATTACK_OMEN,        // �U���\��

        MAX
    }

    public string[] collisionTags;       // �����蔻��̃^�O
    public string[] collisionLayers;     // �����蔻��̃��C���[
}
