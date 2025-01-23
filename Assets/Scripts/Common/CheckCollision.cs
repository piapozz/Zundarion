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
    private string playerTag = null;
    private string enemyTag = null;
    private string thisTag = null;

    public List<BaseCharacter> parryList { get; private set; } = new List<BaseCharacter>(3);

    private BaseCharacter _character = null;

    private void Start()
    {
        playerTag = "Player";
        enemyTag = "Enemy";
        thisTag = gameObject.tag;
        _character = GetComponent<BaseCharacter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject hitObj = other.gameObject;
        string hitTagName = hitObj.tag;

        // ���肷�ׂ��R���W����������
        CollisionData collisionData = hitObj.GetComponent<CollisionData>();
        if (collisionData == null || !JudgeTag(hitTagName)) return;

        bool isParry = collisionData.isParry;
        int hitID = collisionData.characterID;
        BaseCharacter hitCharacter = CharacterManager.instance.GetCharacter(hitID);
        if (hitCharacter == null) return;
        // �p���B�Ń��X�g�ɓ����Ă��Ȃ��Ȃ�
        if (isParry)
        {
            if (parryList.Exists(x => x == hitCharacter)) return;

            // ���X�g�ɓ����
            parryList.Add(hitCharacter);
        }
        else
        {
            // �_���[�W����
            hitCharacter.TakeDamage(collisionData.damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �p���B���X�g����폜
        GameObject hitObj = other.gameObject;
        BaseCharacter character = hitObj.GetComponent<BaseCharacter>();
        if (parryList.Exists(x => x == character))
            parryList.Remove(character);
    }

    /// <summary>
    /// �G�̃^�O�����肷��
    /// </summary>
    /// <param name="hitTag"></param>
    /// <returns></returns>
    private bool JudgeTag(string hitTag)
    {
        if ((hitTag == enemyTag && thisTag == playerTag) ||
            (hitTag == playerTag && thisTag == enemyTag))
            return true;
        else
            return false;
    }
}
