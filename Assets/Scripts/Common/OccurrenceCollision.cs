using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class OccurrenceCollision : StateMachineBehaviour
{
    // private //////////////////////////////////////////////////////////////////

    /// <summary>�v���C���[�̈ړ��f�[�^</summary>
    [SerializeField]
    private CollisionAction _collisionAction = null;

    /// <summary>�v���C���[�̓����蔻�蔭�����</summary>
    [SerializeField]
    private float _occurrenceTime;

    /// <summary>�_���[�W��</summary>
    [SerializeField]
    private float _damage;

    /// <summary>���������郌�C���[</summary>
    [SerializeField]
    private CollisionAction.CollisionLayer layer;

    /// <summary>����������^�O</summary>
    [SerializeField]
    private CollisionAction.CollisionTag tag;

    /// <summary>��ԓ��ň�x�����������邽�߂̃t���O</summary>
    private bool _hasGeneratedCollision = false;

    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ����̃^�C�~���O�����Ă��āA���܂��������Ă��Ȃ��ꍇ�̂ݎ��s
        if (!_hasGeneratedCollision && stateInfo.normalizedTime > _occurrenceTime)
        {
            // �����蔻��𐶐�����
            CreateCollision.AttackData data = new CreateCollision.AttackData().zero();

            data.position = animator.transform.position;
            data.radius = 2;
            data.layer = _collisionAction.collisionLayers[(int)layer];
            data.tagname = _collisionAction.collisionTags[(int)tag];
            data.time = 2;
            data.damage = _damage;

            // ����
            CreateCollision.instance.CreateCollisionSphere(animator.gameObject, data);

            // ��x�������s���邽�߃t���O�𗧂Ă�
            _hasGeneratedCollision = true;
        }
       
    }
    public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ��ԊJ�n���Ƀt���O�����Z�b�g
        _hasGeneratedCollision = false;
    }
}
