using UnityEngine;

public interface IEnemyState
{
    // ��Ԃɉ����Ď��s��
    public EnemyBase.EnemyStatus Action(EnemyBase.EnemyStatus enemyStatus);
}
