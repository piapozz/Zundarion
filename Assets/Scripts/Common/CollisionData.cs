/*
* @file CollisionData.cs
* @brief �����蔻��ɃA�^�b�`���铖���蔻��̃f�[�^
* @date 2025/1/20
*/

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CommonModule;

public class CollisionData : MonoBehaviour
{
    public int characterID = -1;
    public bool isParry = false;
    public float damage = -1;
    public float deleteTime = 0.1f;

    /// <summary>
    /// �������Ԃ�ݒ肵�A���Ԍo�ߌ�ɃQ�[���I�u�W�F�N�g��j�󂵂܂��B
    /// </summary>
    /// <param name="time">�������ԁi�b�j</param>
    public void OnEnable()
    {
        UniTask task = WaitAction(deleteTime, LimitOver);
    }

    /// <summary>
    /// �A�^�b�`����Ă���I�u�W�F�N�g���A�N�e�B�u�ɂ���
    /// </summary>
    private void LimitOver()
    {
        CollisionManager.instance.UnuseCollision(gameObject);
        BaseCharacter parryTarget = CharacterManager.instance.GetCharacter(characterID);
        CollisionManager.instance.parryList.Remove(parryTarget);
    }
}
