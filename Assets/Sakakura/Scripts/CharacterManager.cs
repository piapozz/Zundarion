/*
* @file CharacterManager.cs
* @brief �L�����N�^�[�̊Ǘ�
* @author sakakura
* @date 2025/1/22
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : SystemObject
{
    public static CharacterManager instance { get; private set; } = null;

    private static readonly int ENEMY_MAX = 10;

    [SerializeField]
    private GameObject playerOrigin = null;

    [SerializeField]
    public DamageObserver _damageObserver = null;

    public BasePlayer player { get; private set; } = null;

    public List<BaseCharacter> characterList { get; private set; } = null;

    public override void Initialize()
    {
        instance = this;
        characterList = new List<BaseCharacter>(ENEMY_MAX + 1);
        for (int i = 0, max = ENEMY_MAX + 1; i < max; i++)
        {
            characterList.Add(null);
        }

        _damageObserver = new CreateDamageEffect();

        GeneratePlayer(playerOrigin, StageManager.instance._startTrasform);
    }

    /// <summary>
    /// �v���C���[�𐶐�����
    /// </summary>
    public void GeneratePlayer(GameObject genBase, Transform genTransform)
    {
        BaseCharacter genCharacter = GenerateCharacter(genBase, genTransform);
        player = genCharacter as BasePlayer;
    }

    /// <summary>
    /// �G�𐶐�����
    /// </summary>
    /// <param name="generateNum"></param>
    public void GenerateEnemy(GameObject genBase, Transform genTransform)
    {
        BaseCharacter genCharacter = GenerateCharacter(genBase, genTransform);

        UIManager.instance.AddEnemyUI(genCharacter as BaseEnemy);
    }

    /// <summary>
    /// �L�����N�^�[�𐶐�����
    /// </summary>
    /// <param name="genBase"></param>
    /// <param name="genTransform"></param>
    /// <returns></returns>
    private BaseCharacter GenerateCharacter(GameObject genBase, Transform genTransform)
    {
        GameObject genCharacter = null;
        BaseCharacter character = null;
        int useID = GetEmptyID();
        if (useID < 0)
        {
            useID = characterList.Count;
            genCharacter = Instantiate(genBase);
            character = genCharacter.GetComponent<BaseCharacter>();
            character.Initialize(useID);
            characterList.Add(character);
        }
        else
        {
            genCharacter = Instantiate(genBase);
            character = genCharacter.GetComponent<BaseCharacter>();
            character.Initialize(useID);
            characterList[useID] = character;
        }
        character.SetTransform(genTransform);
        character.SetDamageObserver(_damageObserver);
        return character;
    }

    /// <summary>
    /// ��̃L�����N�^�[���X�g�̔ԍ����擾
    /// </summary>
    /// <returns></returns>
    private int GetEmptyID()
    {
        int ID = -1;
        for (int i = 0, max = characterList.Count; i < max; i++)
        {
            if (characterList[i] != null)
                continue;
            ID = i;
            break;
        }
        return ID;
    }

    /// <summary>
    /// �L�����N�^�[�̃��X�g����L����������
    /// </summary>
    /// <param name="character"></param>
    public void RemoveCharacterList(int ID)
    {
        characterList[ID] = null;
    }

    /// <summary>
    /// BaseCharacter�N���X���擾����
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public BaseCharacter GetCharacter(int ID)
    {
        if (characterList[ID] == null) return null;

        return characterList[ID].GetComponent<BaseCharacter>();
    }

    /// <summary>
    /// �G�l�~�[�����邩����
    /// </summary>
    /// <returns></returns>
    public bool IsAliveEnemy()
    {
        for (int i = 0, max = characterList.Count; i < max; i++)
        {
            if (characterList[i] == null || characterList[i].IsPlayer())
                continue;
            return true;
        }
        return false;
    }

    /// <summary>
    /// �w��̍��W���犴�m�͈͓��̃L�����N�^�[���擾
    /// </summary>
    /// <param name="position"></param>
    /// <param name="sensRange"></param>
    /// <returns></returns>
    public BaseCharacter GetNearCharacter(BaseCharacter character, float sensRange)
    {
        float minDistance = float.MaxValue;
        Vector3 position = character.gameObject.transform.position;
        BaseCharacter nearCharacter = null;
        // �L�����N�^�[���X�g�𑖍�
        for (int i = 0, max = characterList.Count; i < max; i++)
        {
            // �������g�͏��O
            if (character == characterList[i]) continue;

            if (characterList[i] == null) continue;

            GameObject target = characterList[i].gameObject;
            if (target == null) continue;
            float distanceToCharacter = Vector3.Distance(position, target.transform.position);
            if (distanceToCharacter > sensRange) continue;
            // �ŏ��������X�V
            if (distanceToCharacter < minDistance)
            {
                minDistance = distanceToCharacter;
                nearCharacter = characterList[i];
            }
        }

        return nearCharacter;
    }
}
