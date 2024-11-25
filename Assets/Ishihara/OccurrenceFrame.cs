using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/OccurrenceFrame")]
public class OccurrenceFrame : ScriptableObject
{
    public float[] attackOccurrenceFrame;       // �U���̓����蔻�蔭���b��
    public float avoidanceOccurrenceFrame;      // ����̓����蔻�蔭���b��
    public float parryOccurrenceFrame;          // �p���B�̓����蔻�蔭���b��
}
