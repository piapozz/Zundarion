/*
* @file CollisionData.cs
* @brief �����蔻��ɃA�^�b�`���铖���蔻��̃f�[�^
* @date 2025/1/20
*/

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static CommonModule;
using static GameConst;

public class CollisionData : MonoBehaviour
{
    public int characterID = -1;
    public bool isParry = false;
    public bool isAvoid = false;
    public float damage = -1;
    public int deleteFrame = -1;

    private string thisTag = null;

    /// <summary>
    /// �������Ԃ�ݒ肵�A���Ԍo�ߌ�ɃQ�[���I�u�W�F�N�g��j�󂵂܂��B
    /// </summary>
    /// <param name="time">�������ԁi�b�j</param>
    public void OnEnable()
    {
        thisTag = transform.tag;
        UniTask task = WaitAction(deleteFrame, LimitOver);
    }

    // �e�I�u�W�F�N�g��destroy��List��������ĂȂ��H
    private void OnDisable()
    {
        RemoveList();
    }

    /// <summary>
    /// �A�^�b�`����Ă���I�u�W�F�N�g���A�N�e�B�u�ɂ���
    /// </summary>
    private void LimitOver()
    {
        RemoveList();
        CollisionManager.instance.UnuseCollision(gameObject);
    }

    private readonly string PLAYER_TAG = "Player";
    private readonly string ENEMY_TAG = "Enemy";

    private void OnTriggerEnter(Collider other)
    {
        // �^�O�Ń`�F�b�N����
        GameObject hitObj = other.gameObject;
        string hitTagName = hitObj.tag;
        if (!JudgeHittable(hitTagName)) return;

        // BaseCharacter���擾
        BaseCharacter hitCharacter = hitObj.GetComponent<BaseCharacter>();
        if (hitCharacter == null) return;

        BaseCharacter sourceCharacter = CharacterManager.instance.GetCharacter(characterID);
        if (sourceCharacter == null) return;
        if (isParry)
        {
            // ���X�g�ɓ����
            CharacterManager.instance.player.AddParryList(this);
        }
        else if (isAvoid)
        {
            // ���X�g�ɓ����
            CharacterManager.instance.player.AddAvoidList(this);
        }
        else
        {
            // �_���[�W����
            hitCharacter.TakeDamage(damage, sourceCharacter.strength);
            sourceCharacter.SetAnimationSpeed(HIT_STOP_SPEED, HIT_STOP_FRAME);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isParry && !isAvoid) return;
        // �^�O�Ń`�F�b�N����
        GameObject hitObj = other.gameObject;
        string hitTagName = hitObj.tag;
        if (!JudgeHittable(hitTagName)) return;

        RemoveList();
    }

    /// <summary>
    /// �G�̃^�O�����肷��
    /// </summary>
    /// <param name="hitTag"></param>
    /// <returns></returns>
    private bool JudgeHittable(string hitTag)
    {
        if ((hitTag == ENEMY_TAG && thisTag == PLAYER_TAG) ||
            (hitTag == PLAYER_TAG && thisTag == ENEMY_TAG))
            return true;
        else
            return false;
    }

    private void RemoveList()
    {
        if (isParry)
        {
            CharacterManager.instance.player.RemoveParryList(this);
        }
        else if (isAvoid)
        {
            CharacterManager.instance.player.RemoveAvoidList(this);
        }
    }
}
