using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using static EnemyBase.EnemyStatus;
using Unity.VisualScripting;

// 親クラスのpublicをインスペクター上に表示
#if UNITY_EDITOR
[CustomEditor(typeof(EnemyBase))]
#endif

public class EnemyBear : EnemyBase
{    
    // 敵の行動
    // ※ダメージ処理を最後にしないとステートが更新されて生き返ったりする
    protected override void UpdateEnemy()
    {
        // Rayを飛ばしてPlayerタグだったものの情報を取得
        // GetPlayerObject();

        // 行動する
        status = actionState.Action(status);

        // プレイヤーの方を向く
        gameObject.transform.rotation = status.m_toPlayerAngle;
        
        // Eif(transform.position.y < 0.0f) transform.position = status.m_position;

        // ステートの変更があればステートを変更
        if (oldState != status.m_state) SetState(status.m_state);

        // ブレイク値が溜まったら
        if (status.m_breakMax <= status.m_break)
        {
            // 状態の切り替え
            // status.m_state = new BearDown();
        }

        // ダメージ処理
        if(status.m_health <= 0 )
        {
            status.m_state = (int)EnemyBase.EnemyStatus.ActionState.STATE_DEAD;
        }

        // アニメーションの再生が終わったら自身を消滅
        if (status.m_dead == true) Destroy(this.gameObject);
    }

    protected override void Init()
    {
        // 状態がなかったら待機状態で初期化する
        if (actionState == null) actionState = new BearIdle();
    }
}
