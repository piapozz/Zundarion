/*
 * @file MoveDataData.cs
 * @brief MoveEventで使う情報をまとめる
 * @author sein
 * @date 2025/1/27
 */

using UnityEngine;

[CreateAssetMenu(fileName = "MoveEventData", menuName = "Event/MoveEventData")]
public class MoveEventData : ScriptableObject
{
    public Vector3 dir;
    public int frame;
    public float length;
    public bool isApproach;
    public float maxLength;
    public float minLength;
}
