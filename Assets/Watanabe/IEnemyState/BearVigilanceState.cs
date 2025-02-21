using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BearVigilanceState : BaseEnemyState
{
    public Transform player;   // プレイヤー
    public float radius = 5f;  // 保持したい距離
    public float speed = 0.5f;   // 移動速度
    public float radiusCorrectionSpeed = 2f; // 半径補正速度

    private float angle = 0f;  // 現在の角度

    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Vigilance", true);
    }

    public override void Execute(BaseEnemy enemy)
    {
        //// 角度を更新
        //// angle += speed * Time.deltaTime;

        //angle = 0;

        player = CharacterManager.instance.characterList[0].transform;

        Vector3 enemyPosition = enemy.GetEnemyPosition();

        /*
        // 現在の敵の位置とプレイヤーの位置の距離を維持しながら新しい方向を求める
        //Vector3 offset = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        //Vector3 targetPos = player.position + offset;

        //// 移動方向を求める（正規化して渡す）
        //Vector3 moveDir = (targetPos - enemy.GetEnemyPosition()).normalized;

        //enemy.Rotate(enemy.GetTargetVec(player.position));
        //enemy.Move(speed, moveDir);
        */

        // **現在のプレイヤーとの距離を取得**
        float currentRadius = Vector3.Distance(enemyPosition, player.position);

        // **円運動の角度を更新**
        angle += speed * Time.deltaTime;

        // **目標位置を計算（プレイヤー中心の円軌道）**
        Vector3 offset = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        Vector3 targetPos = player.position + offset;

        // **移動方向を求める（プレイヤーから離れる成分を加える）
        Vector3 moveDir = (targetPos - enemyPosition).normalized;

        // **距離がズレている場合、補正**
        if (Mathf.Abs(currentRadius - radius) > 0.1f)  // 許容範囲を超えていたら補正
        {
            Vector3 correction = (enemyPosition - player.position).normalized * (radius - currentRadius);
            moveDir += correction;  // 外向きの力を加える
            moveDir.Normalize();  // 正規化して余分な速度をなくす
        }

        // **Move() を呼び出し、dir に適切な方向を渡す**
        enemy.Move(speed, moveDir);

        Vector3 targetVec = enemy.GetTargetVec(player.position);
        enemy.Rotate(targetVec);

    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Vigilance", false);
    }
}
