/*
 * @file MoveDataData.cs
 * @brief MoveEvent�Ŏg�������܂Ƃ߂�
 * @author sein
 * @date 2025/1/27
 */

using UnityEngine;

[CreateAssetMenu(fileName = "RotateEventData", menuName = "Event/RotateEventData")]
public class RotateEventData : ScriptableObject
{
    public Vector3 dir;
    public int frame;
    public float speed;
}

