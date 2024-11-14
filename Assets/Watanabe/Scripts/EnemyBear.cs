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
    public enum State
    {
        ENEMY_IDLE = 0,               // 待機状態
        ENEMY_FOUND,                  // 見つけたとき
        ENEMY_TRACKING,               // 距離が離れたとき
        ENEMY_TURN,                   // 距離は近いが攻撃範囲から外れたとき
        ENEMY_ATTACK,                 // 通常攻撃
        ENEMY_ATTACK_UNIQUE,          // 特定の条件下でする攻撃
        ENEMY_DEAD,                   // 倒されたとき

        MAX
    }


    // 敵の行動
    protected override void UpdateEnemy()
    {
        
    }
}
