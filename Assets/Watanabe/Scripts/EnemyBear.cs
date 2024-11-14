using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

// 親クラスのpublicをインスペクター上に表示
#if UNITY_EDITOR
[CustomEditor(typeof(EnemyBase))]
#endif

public class EnemyBear : EnemyBase
{

    int count = 0;

    // 敵の
    public enum StateBear
    {
        STATE_IDLE = 0,               // 待機状態
        STATE_FOUND,                  // 見つけたとき
        STATE_TRACKING,               // 距離が離れたとき
        STATE_TURN,                   // 距離は近いが攻撃範囲から外れたとき
        STATE_ATTACK,                 // 通常攻撃
        STATE_ATTACK_UNIQUE,          // 特定の条件下でする攻撃
        STATE_DEAD,                   // 倒されたとき

        MAX
    }
    
    // 敵の行動
    protected override void UpdateEnemy()
    {
        // 行動する
        status.m_state.Action(status);

        // 条件でステートの変更
        // 敵が近かったら
        if (1 == 1) status.m_state = new BearAttack();

        


    }

    protected override void Init()
    {
        // 状態がなかったら待機状態で初期化する
        if (status.m_state == null) status.m_state = new BearIdle();
    }
}
