using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAttack", menuName = "CharacterAttack")]
public class CharacterAttackData : ScriptableObject
{
    public Vector3 generateOffset;
    public float radius;
    public float damage;

}
