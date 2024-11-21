using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BearDown : IEnemyState
{
    public EnemyBase.EnemyStatus Action(EnemyBase.EnemyStatus enemyStatus)
    {

        //// ブレイク値が回復したら状態を戻す
        //if()
        //{
        //    enemyStatus.m_state = new BearTracking();
        //}

        return enemyStatus;
    }
}
