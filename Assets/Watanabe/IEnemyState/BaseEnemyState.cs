using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyState : IEnemyState
{
    protected BaseEnemy enemy; // 現在の敵オブジェクトを参照

    public void SetEnemy(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }

    protected float GetDistance(Transform _transform)
    {
        return enemy.GetRelativePosition(_transform).magnitude;
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
