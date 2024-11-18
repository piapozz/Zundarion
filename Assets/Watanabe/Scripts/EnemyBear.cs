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
    // 敵の
    public enum StateBear
    {
        STATE_IDLE = 0,               // 待機状態
        STATE_FOUND,                  // 見つけたとき
        STATE_TRACKING,               // 距離が離れたとき
        STATE_TURN,                   // 距離は近いが攻撃範囲から外れたとき
        STATE_ATTACK,                 // 通常攻撃
        STATE_ATTACK_UNIQUE,          // 特定の条件下でする攻撃
        STATE_DOWN,                   // ダウン状態
        STATE_DEAD,                   // 倒されたとき

        MAX
    }
    
    // 敵の行動
    // ※ダメージ処理を最後にしないとステートが更新されて生き返ったりする
    protected override void UpdateEnemy()
    {
        // Rayを飛ばしてPlayerタグだったものの情報を取得
        // GetPlayerObject();

        // 行動する
        status.m_state.Action(status);

        // ブレイク値が溜まったら
        if (status.m_breakMax <= status.m_break)
        {
            // 状態の切り替え
            status.m_state = new BearDown();
        }

        // ダメージ処理
        if(status.m_health <= 0)
        {
            status.m_state = new BearDead();
        }

        // アニメーションの再生が終わったら自身を消滅
        if (status.m_dead == true) Destroy(this.gameObject);
    }

    protected override void Init()
    {
        // 状態がなかったら待機状態で初期化する
        if (status.m_state == null) status.m_state = new BearIdle();
    }
}
