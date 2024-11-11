using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// 親クラスのpublicをインスペクター上に表示
#if UNITY_EDITOR
[CustomEditor(typeof(EnemyBase))]
#endif

public class EnemyBear : EnemyBase
{
    // 敵の行動
    protected override void UpdateEnemy()
    {

    }
}
