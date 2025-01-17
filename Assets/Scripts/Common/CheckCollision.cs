/*
* @file CheckCollision.cs
* @brief �L�����N�^�[�ɃA�^�b�`���Ĕ��������
* @author sein
* @date 2025/1/17
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    [SerializeField] CollisionAction collisionAction;

    public bool canParry { get; private set; } = false;
    private string playerTag = null;
    private string enemyTag = null;
    private string thisTag = null;

    public List<GameObject> parryList { get; private set; } = new List<GameObject>(3);

    private void Start()
    {
        playerTag = collisionAction.collisionTags[(int)CollisionAction.CollisionTag.PLAYER];
        enemyTag = collisionAction.collisionTags[(int)CollisionAction.CollisionTag.ENEMY];
        thisTag = transform.parent.gameObject.tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject hitObj = other.gameObject;
        string hitTagName = hitObj.tag;
        string hitLayerName = LayerMask.LayerToName(hitObj.layer);
        // �G�̃R���W����������
        if (!JudgeTag(hitTagName)) return;

        // �p���B���X�g�ɓ����
        if (JudgeParry(hitLayerName) && !parryList.Exists(x => x == hitObj))
        {
            parryList.Add(hitObj);
            return;
        }

        // �_���[�W����
        TakeDamage(hitObj, hitLayerName);
    }

    private void OnTriggerExit(Collider other)
    {
        // �p���B���X�g����폜
        GameObject hitObj = other.gameObject;
        if (parryList.Exists(x => x == hitObj))
            parryList.Remove(hitObj);
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

    private bool JudgeParry(string hitLayerName)
    {
        // �ڐG�����R���W�������U���\��������
        if (hitLayerName == collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.ATTACK_OMEN])
            return true;
        else
            return false;
    }

    private void TakeDamage(GameObject hitObj, string hitLayerName)
    {
        if (hitLayerName != collisionAction.collisionLayers[(int)CollisionAction.CollisionLayer.ATTACK_NORMAL])
            return;

        float damage = hitObj.GetComponent<DealDamage>().damage;
        gameObject.BaseCharacter.TakeDamage(damage);
    }
}
