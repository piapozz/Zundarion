using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Editorã‚©‚çì¬‚·‚é‚½‚ß‚Ì‹Lq
[CreateAssetMenu(menuName = "ScriptableObjects/Enemy/EnemyStatusObject")]

public class InitialStatus : ScriptableObject
{
    public string aiName;

    public int health;
    public int speed;
    public int power;
}
