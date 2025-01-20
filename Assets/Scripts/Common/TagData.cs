using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TagData")]
public class TagData : ScriptableObject
{
    public enum CollisionTag
    {
        PLAYER,      // �v���C���[
        ENEMY,       // �G�l�~�[

        MAX
    }

    public string[] collisionTags;       // �����蔻��̃^�O
}
