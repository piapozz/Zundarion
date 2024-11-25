using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class OccurrenceCollision : StateMachineBehaviour
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>�v���C���[�̓����蔻����</summary>
    [SerializeField]
    private CollisionAction _collisionPram = null;

    /// <summary>�v���C���[�̓����蔻�蔭�����</summary>
    [SerializeField]
    private float _occurrenceTime;

    /// <summary>���������郌�C���[</summary>
    [SerializeField]
    private CollisionAction.CollisionLayer layer;

    /// <summary>����������^�O</summary>
    [SerializeField]
    private CollisionAction.CollisionTag tag;

    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���ݍĐ�����Ă��鎞�Ԃ�����̃^�C�~���O��������B
        if (stateInfo.normalizedTime >= _occurrenceTime)
        {
            // �����蔻��𐶐�����
            // �p�����[�^�[������
            CreateCollision.AttackData data = new CreateCollision.AttackData().zero();

            data.position = animator.transform.position;
            data.radius = 2;
            data.layer = _collisionPram.collisionLayers[(int)layer];
            data.tagname = _collisionPram.collisionTags[(int)tag];
            data.time = 2;

            // ����
            CreateCollision.instance.CreateCollisionSphere(animator.gameObject, data);
        }
    }
}
