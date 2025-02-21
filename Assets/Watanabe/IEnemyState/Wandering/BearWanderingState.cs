using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BearWanderingState : BaseEnemyState
{
    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Wandering", true);
        SetEnemy(enemy);
    }

    public override void Execute(BaseEnemy enemy)
    {
        Transform playerTransform = CharacterManager.instance.characterList[0].transform;

        // 距離を見てステートを変更
        if (GetDistance(playerTransform) <= 3.0f)
        {
            enemy.ChangeState(new BearFoundState());
        }
    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Wandering", false);
    }
}
