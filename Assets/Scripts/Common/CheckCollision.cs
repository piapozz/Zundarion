/*
* @file CheckCollision.cs
* @brief �L�����N�^�[�ɃA�^�b�`���Ĕ��������
* @author sein
* @date 2025/1/17
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CheckCollision : MonoBehaviour
{
    private readonly string PLAYER_TAG = "Player";
    private readonly string ENEMY_TAG = "Enemy";
    private string _thisTag = null;

    private void Start()
    {
        _thisTag = gameObject.tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        // �^�O�Ń`�F�b�N����
        GameObject hitObj = other.gameObject;
        string hitTagName = hitObj.tag;
        if (!JudgeTag(hitTagName)) return;

        // CollisionData���擾
        CollisionData collisionData = hitObj.GetComponent<CollisionData>();
        if (collisionData == null) return;

        // ID����BaseCharacter���擾
        int hitID = collisionData.characterID;
        BaseCharacter hitCharacter = CharacterManager.instance.GetCharacter(hitID);
        if (hitCharacter == null) return;

        bool isParry = collisionData.isParry;
        if (isParry)
            // ���X�g�ɓ����
            CollisionManager.instance.parryList.Add(hitCharacter);
        else
            // �_���[�W����
            hitCharacter.TakeDamage(collisionData.damage);
    }

    private void OnTriggerExit(Collider other)
    {
        // �^�O�Ń`�F�b�N����
        GameObject hitObj = other.gameObject;
        string hitTagName = hitObj.tag;
        if (!JudgeTag(hitTagName)) return;

        // CollisionData���擾
        CollisionData collisionData = hitObj.GetComponent<CollisionData>();
        if (collisionData == null) return;

        // ID����BaseCharacter���擾
        int hitID = collisionData.characterID;
        BaseCharacter hitCharacter = CharacterManager.instance.GetCharacter(hitID);
        if (hitCharacter == null) return;

        bool isParry = collisionData.isParry;
        if (isParry)
            // ���X�g����O��
            CollisionManager.instance.parryList.Remove(hitCharacter);
    }



    /// <summary>
    /// �G�̃^�O�����肷��
    /// </summary>
    /// <param name="hitTag"></param>
    /// <returns></returns>
    private bool JudgeTag(string hitTag)
    {
        if ((hitTag == ENEMY_TAG && _thisTag == PLAYER_TAG) ||
            (hitTag == PLAYER_TAG && _thisTag == ENEMY_TAG))
            return true;
        else
            return false;
    }
}
