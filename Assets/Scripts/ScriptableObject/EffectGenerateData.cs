using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectGenerateData", menuName = "ScriptableObjects/EffectGenerateData")]
public class EffectGenerateData : ScriptableObject
{
    public int ID;

    public Vector3 dir;

    public float length;
}
