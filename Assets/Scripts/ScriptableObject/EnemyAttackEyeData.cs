using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackEyeData", menuName = "ScriptableObjects/EnemyAttackEyeData")]
public class EnemyAttackEyeData : ScriptableObject
{
    // ƒpƒŠƒB‚Å‚«‚é‚©‚Ç‚¤‚©
    public bool isParry;
    // ‰ñ”ğ‚Å‚«‚é‚©‚Ç‚¤‚©
    public bool isAvoid;
    // Á‚·‚Ü‚Å‚ÌŠÔ
    public float deleteTime;
}
