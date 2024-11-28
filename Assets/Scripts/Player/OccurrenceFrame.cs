using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/OccurrenceFrame")]
public class OccurrenceFrame : ScriptableObject
{
    public float[] attackOccurrenceFrame;       // UŒ‚‚Ì“–‚½‚è”»’è”­¶•b”
    public float avoidanceOccurrenceFrame;      // ‰ñ”ğ‚Ì“–‚½‚è”»’è”­¶•b”
    public float parryOccurrenceFrame;          // ƒpƒŠƒB‚Ì“–‚½‚è”»’è”­¶•b”
}
