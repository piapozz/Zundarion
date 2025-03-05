using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyState : IEnemyState
{
    protected BaseEnemy enemy; // 現在の敵オブジェクトを参照

    protected readonly float _TRANITION_TIME = 1.5f;
    protected readonly float _ENEMY_DISTANCE = 4.0f;
    protected readonly float _ENEMY_DISTANCE_NEAR = 4.0f;
    protected readonly float _ENEMY_DISTANCE_FAR = 10.0f;

    protected BasePlayer player = CharacterManager.instance.player;

    public void SetEnemy(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }

    protected float GetDistance(Transform _transform)
    {
        return enemy.GetRelativePosition(_transform).magnitude;
    }

    protected Vector3 GetTargetVec(Transform _transform)
    {
        return enemy.GetTargetVec(_transform.position);
    }

    protected int RandomNumber(int maxValue)
    {
        if (maxValue <= 0.0f) return -1;
        return Random.Range(0, maxValue);
    }

    public virtual void Enter(BaseEnemy enemy) { }
    public abstract void Execute(BaseEnemy enemy);
    public virtual void Exit(BaseEnemy enemy) { }


}
