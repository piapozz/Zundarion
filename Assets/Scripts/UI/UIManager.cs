/*
* @file UIManager.cs
* @brief �Q�[����UI���Ǘ�����N���X
* @author sein
* @date 2025/1/31
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �o����ΓG�̍ő�̗͂�����UI��L�΂�
// �̗͂�����܂Ń��O�����
// Pooling�Ŏ�������

// ��Canvas��G�̐�������������͕̂`��R�X�g�������A���ׂɂȂ���

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; } = null;

    private readonly int INITIAL_COUNT = 5;

    [SerializeField] private GameObject enemyUI;
    [SerializeField] private GameObject canvasWorldSpace;

    private CharacterManager characterManager = null;
    private Camera mainCamera = null;

    // �g�p���̃L�����N�^�[���X�g
    private List<BaseCharacter> useList = null;
    private List<BasePlayer> unusePlayer = null;
    private List<BaseEnemy> unuseEnemyList = null;

    private List<GameObject> useObjectList = null;
    private List<GameObject> unuseObjectList = null;

    Vector3 viewportPos;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        characterManager = CharacterManager.instance;
        mainCamera = CameraManager.instance.selfCamera;

        // �S�Ă��󂾂����炠�炩���ߐ�������
        if (useObjectList == null && unuseObjectList == null)
        {
            useObjectList = new List<GameObject>();
            unuseObjectList = new List<GameObject>();

            // Canvas��transform���擾
            var parent = canvasWorldSpace.transform;

            for (int i = 0, max = INITIAL_COUNT; i < max; i++)
            {
                AddEnemyUI(parent);
            }
        }
    }

    private void Update()
    {
        List<BaseCharacter> characterList = characterManager.characterList;

        for (int i = 0, max = characterList.Count; i < max; i++) 
        {
            //Debug.Log("temae");
            //if (characterList[i].GetComponent<BasePlayer>()) break;
            //Debug.Log("�ʂ���");
            //viewportPos = mainCamera.WorldToViewportPoint(characterList[i].transform.position + Vector3.up * 2f);

            //if (viewportPos.z > 0 && viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1)
            //{
            //    Debug.Log("aaaaa");
            //}
            //else
            //{
            //    Debug.Log("iii");
            //}
        }

        // �g�p�����Ƃ��ɕK�v�ȏ���n���ĕ\������

        // ����UI���Z�b�g����Ă��Ȃ��G������������
        if (true)
        {

        }

        // �g�p����Ă��Ȃ��I�u�W�F�N�g�����������\���ɂ���
        for (int i = 0, max = unuseObjectList.Count; i < max; i++) 
        {
            // ��\���ɂ���Ă��Ȃ��I�u�W�F�N�g����������
            if(unuseObjectList[i].activeSelf != false)
            {
                SetActive(unuseObjectList[i], false);
            }
        }

        // UI���X�V����
        for (int i = 0, max = useObjectList.Count; i < max; i++)
        {

            if(useObjectList[i] != null)
            {

            }
        }
    }

    // ���X�g��UI��ǉ�����
    private void AddEnemyUI(Transform transform)
    {
        // �q�Ƃ��Đ���
        unuseObjectList.Add(Instantiate(enemyUI, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, transform));
    }

    /// <summary>
    /// �w�肳�ꂽ�I�u�W�F�N�g�̃A�N�e�B�u�؂�ւ��֐�
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(GameObject gameObj, bool active)
    {
        gameObj.SetActive(active);
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    time += Time.deltaTime;

    //    //// �v���C���[�̗̑͂��Q�[�W�ŕϓ�������
    //    for (int i = 0; i < playerHealthBar.Length; i++)
    //    {
    //        // VariableBar(playerHealthBar[i], MAX_TIME, basePlayer[i].selfCurrentHealth);
    //        VariableBar(playerHealthBar[(int)TeamMember.MEMBER_1], zundaObject.selfCurrentHealth, 100.0f);
    //        VariableBar(playerHealthBar[(int)TeamMember.MEMBER_2], zunkoObject.selfCurrentHealth, 100.0f);
    //    }

    //    //// �v���C���[�̃X�L���Q�[�W��ϓ�������
    //    //for (int i = 0; i < playerSkillBar.Length; i++)
    //    //{
    //    //    VariableBar(playerSkillBar[i], MAX_TIME, MAX_TIME - time);
    //    //}

    //    //�G�̗̑͂��Q�[�W�ŕϓ�������
    //    for (int i = 0; i < enemyHealthBar.Length; i++)
    //    {
    //        //VariableBar(enemyHealthBar[i], enemyBear.status.m_health, enemyBear.status.m_healthMax);
    //        //Debug.Log(enemyBear.status.m_health);
    //        //Debug.Log(enemyBear.status.m_healthMax);
    //    }

    //    //// �G�̃u���C�N�l�̗��܂���ϓ�������
    //    //for (int i = 0; i < enemyBreakBar.Length; i++)
    //    //{

    //    //}
    //}

}
