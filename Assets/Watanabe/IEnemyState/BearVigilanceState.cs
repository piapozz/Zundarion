using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BearVigilanceState : BaseEnemyState
{
    public Transform player;   // ÉvÉåÉCÉÑÅ[
    private float speed = 1.0f;
    private float _count = 0;
    private readonly float _ENEMY_VIGILANCE_TIME = 5.0f;

    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Vigilance", true);
        SetEnemy(enemy);
    }

    public override void Execute(BaseEnemy enemy)
    {
        _count += Time.deltaTime;
        player = CharacterManager.instance.characterList[0].transform;
        Vector3 targetVec = enemy.GetTargetVec(player.position);

        enemy.Move(speed, Vector3.left);
        enemy.Rotate(targetVec);

        if (_count < _ENEMY_VIGILANCE_TIME) return;

        Transform playerTransform = CharacterManager.instance.characterList[0].transform;
        float distance = GetDistance(playerTransform);

        // ãﬂÇ©Ç¡ÇΩÇÁ
        if (distance >= _ENEMY_DISTANCE_FAR) { enemy.ChangeState(new BearChasingState()); }
        else if (distance <= _ENEMY_DISTANCE_NEAR) { enemy.ChangeState(new BearAttackState()); }
        else { enemy.ChangeState(new BearHammerState()); }
    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Vigilance", false);
    }
}
