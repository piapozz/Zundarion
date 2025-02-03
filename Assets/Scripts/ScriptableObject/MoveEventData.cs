/*
 * @file MoveDataData.cs
 * @brief MoveEvent‚Åg‚¤î•ñ‚ğ‚Ü‚Æ‚ß‚é
 * @author sein
 * @date 2025/1/27
 */

using UnityEngine;

[CreateAssetMenu(fileName = "MoveEventData", menuName = "Event/MoveEventData")]
public class MoveEventData : ScriptableObject
{
    public Vector3 dir;
    public int frame;
    public float speed;
}
