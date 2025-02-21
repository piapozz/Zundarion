using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BearVigilanceState : BaseEnemyState
{
    public Transform player;   // �v���C���[
    public float radius = 5f;  // �ێ�����������
    public float speed = 0.5f;   // �ړ����x
    public float radiusCorrectionSpeed = 2f; // ���a�␳���x

    private float angle = 0f;  // ���݂̊p�x

    public override void Enter(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Vigilance", true);
    }

    public override void Execute(BaseEnemy enemy)
    {
        //// �p�x���X�V
        //// angle += speed * Time.deltaTime;

        //angle = 0;

        player = CharacterManager.instance.characterList[0].transform;

        Vector3 enemyPosition = enemy.GetEnemyPosition();

        /*
        // ���݂̓G�̈ʒu�ƃv���C���[�̈ʒu�̋������ێ����Ȃ���V�������������߂�
        //Vector3 offset = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        //Vector3 targetPos = player.position + offset;

        //// �ړ����������߂�i���K�����ēn���j
        //Vector3 moveDir = (targetPos - enemy.GetEnemyPosition()).normalized;

        //enemy.Rotate(enemy.GetTargetVec(player.position));
        //enemy.Move(speed, moveDir);
        */

        // **���݂̃v���C���[�Ƃ̋������擾**
        float currentRadius = Vector3.Distance(enemyPosition, player.position);

        // **�~�^���̊p�x���X�V**
        angle += speed * Time.deltaTime;

        // **�ڕW�ʒu���v�Z�i�v���C���[���S�̉~�O���j**
        Vector3 offset = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        Vector3 targetPos = player.position + offset;

        // **�ړ����������߂�i�v���C���[���痣��鐬����������j
        Vector3 moveDir = (targetPos - enemyPosition).normalized;

        // **�������Y���Ă���ꍇ�A�␳**
        if (Mathf.Abs(currentRadius - radius) > 0.1f)  // ���e�͈͂𒴂��Ă�����␳
        {
            Vector3 correction = (enemyPosition - player.position).normalized * (radius - currentRadius);
            moveDir += correction;  // �O�����̗͂�������
            moveDir.Normalize();  // ���K�����ė]���ȑ��x���Ȃ���
        }

        // **Move() ���Ăяo���Adir �ɓK�؂ȕ�����n��**
        enemy.Move(speed, moveDir);

        Vector3 targetVec = enemy.GetTargetVec(player.position);
        enemy.Rotate(targetVec);

    }

    public override void Exit(BaseEnemy enemy)
    {
        enemy.SetAnimatorBool("Vigilance", false);
    }
}
