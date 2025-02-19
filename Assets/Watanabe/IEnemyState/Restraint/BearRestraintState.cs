using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BearRestraintState : BaseEnemyState
{
    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Restraint", true);
    }

    public override void Execute(BaseEnemy enemy)
    {
        Transform playerTransform = CharacterManager.instance.characterList[0].transform;
        float distance = enemy.GetRelativePosition(playerTransform).magnitude;

        if (distance >= 8.0f)
        {
            enemy.ChangeState(new BearWanderingState());
        }

        if (distance <= 3.0f)
        {
            enemy.SetAnimatorTrigger(RandomNumber(2) == 0 ? "Attack" : "StrongAttack");
            enemy.ChangeState(new BearRestraintState());
            return;
        }

        if (distance >= 6.0f && distance <= 10.0f)
        {
            enemy.SetAnimatorTrigger("UniqueAttack");
            return;
        }
    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Restraint", false);
    }

    private static int RandomNumber(int value)
    {
        return Random.Range(0, value);
    }
}
