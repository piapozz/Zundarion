using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConst;
using UnityEngine.UIElements;

public class EnemyHumanRobot : BaseEnemy
{
    // “{‚èƒQ[ƒW
    [SerializeField] private int fury = -1;

    [SerializeField] private Renderer enemyRenderer;
    [SerializeField] private Material barrierMaterial;
    [SerializeField] private Material robotMaterial;
    private readonly int _ENEMY_FURY_INITIAL = 0;
    private readonly int _ENEMY_FURY_INCREASE = 6;

    private void Start()
    {
        ChangeState(new BearIdleState());
        fury = _ENEMY_FURY_INITIAL;
        isInvincible = true;
        enemyHealthColor = Color.cyan;
    }

    private void Update()
    {
        position = transform.position;
    }

    protected override void DamageReaction()
    {
        fury += _ENEMY_FURY_INCREASE;

    }

    public override void SetImpact()
    {
        base.SetImpact();

        isInvincible = false;

        if (isInvincible)
        {
            enemyHealthColor = Color.cyan;
            enemyRenderer.material = barrierMaterial;
        }
        else
        {
            enemyHealthColor = Color.green;
            enemyRenderer.material = robotMaterial;
        }
    }

    public int GetFury() { return fury; }

    // “{‚èƒQ[ƒW‚ð•Ï“®‚³‚¹‚é
    public void ChangeFury(int changeValue)
    {
        if (changeValue == 0) return;

        if (changeValue >= 1) fury += changeValue;

        else if (changeValue <= -1)
        {
            int index = fury + changeValue;
            if (index >= 0) fury = index;
            else if (index <= 0) fury = 0;
        }
    }
}
