using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// �e�N���X��public���C���X�y�N�^�[��ɕ\��
#if UNITY_EDITOR
[CustomEditor(typeof(EnemyBase))]
#endif

public class EnemyBear : EnemyBase
{
    // �G�̍s��
    protected override void UpdateEnemy()
    {

    }
}
