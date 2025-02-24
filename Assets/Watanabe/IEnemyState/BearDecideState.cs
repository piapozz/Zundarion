using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BearDecideState : BaseEnemyState
{
    float count = 0;

    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Decide", true);
        SetEnemy(enemy);
    }

    public override void Execute(BaseEnemy enemy)
    {
        count += Time.deltaTime;

        if (count < 3) return;

        Transform playerTransform = CharacterManager.instance.characterList[0].transform;
        float distance = enemy.GetRelativePosition(playerTransform).magnitude;

        // ‰“‚·‚¬‚½‚ç’Ç‚¢‚©‚¯‚é
        if (GetDistance(playerTransform) >= 20.0f)
        {
            // ’Ç‚¢‚©‚¯‚é
            enemy.ChangeState(new BearChasingState());
        }

        if (GetDistance(playerTransform) <= 5.0f)
        {
            // “{‚èƒ|ƒCƒ“ƒg‚ğŒ©‚ÄDí“I‚¾‚Á‚½‚çUŒ‚
            if (enemy.GetComponent<EnemyBear>().fury >= 50.0f)
            {
                // UŒ‚‚·‚é
                enemy.ChangeState(new BearUpperState());
            }

            // T‚¦–Ú‚¾‚Á‚½‚ç‹}‘¬‚Å‰º‚ª‚é
            else
            {
                // ‰º‚ª‚é
                enemy.ChangeState(new BearJumpState());
            }
        }

        if (GetDistance(playerTransform) >= 10.0f)
        {
            // Dí“I‚¾‚Á‚½‚ç’¼ÚƒWƒƒƒ“ƒvUŒ‚‚·‚é
            if (enemy.GetComponent<EnemyBear>().fury >= 50.0f)
            {
                // UŒ‚‚·‚é
                enemy.ChangeState(new BearJumpAttackState());
            }

            // ŠÔ‡‚¢‚ğæ‚é
            else
            {
                // —lqŒ©‚ğs‚¤
                enemy.ChangeState(new BearVigilanceState());
            }
        }
    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Decide", false);
    }
}