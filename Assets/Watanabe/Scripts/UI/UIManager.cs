using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // �摜���Ǘ�
    [SerializeField] Image[] playerHealthBar;
    [SerializeField] Image[] enemyHealthBar;
    [SerializeField] Image[] playerSkillBar;
    [SerializeField] Image[] enemyBreakBar;

    // PlayerManager
    PlayerManager playerManager;
 
    // �L�����N�^�[
    BasePlayer zundaObject;
    BasePlayer zunkoObject;
    EnemyBase enemyBear;

    float time;

    const float MAX_TIME = 100.0f;

    int[] teamMember;

    // �v���C���[�̃X�e�[�^�X
    struct PlayerStatus
    {
        float m_healthMax;
        float m_health;
        float m_skill;
    }

    enum TeamMember
    {
        MEMBER_1,
        MEMBER_2,
        MEMBER_3,
        MEMBER_MAX
    }

    void Start()
    {
        // PlayerManager���擾
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        // Player���擾
        zundaObject = GameObject.Find("Zundamon").GetComponent<Zunda>();
        zunkoObject = GameObject.Find("Zunko").GetComponent<Zunda>();

        // �G���擾
        enemyBear = GameObject.Find("Bear").GetComponent<EnemyBase>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        // �v���C���[�̗̑͂��Q�[�W�ŕϓ�������
        //for (int i = 0; i < playerHealthBar.Length; i++)
        //{
        //    VariableBar(playerHealthBar[i], MAX_TIME, basePlayer[i].selfCurrentHealth);
        //}

        for (int i = 0; i < playerHealthBar.Length; i++)
        {
            // VariableBar(playerHealthBar[i], MAX_TIME, basePlayer[i].selfCurrentHealth);
            VariableBar(playerHealthBar[(int)TeamMember.MEMBER_1], zundaObject.selfCurrentHealth, 100.0f);
            VariableBar(playerHealthBar[(int)TeamMember.MEMBER_2], zunkoObject.selfCurrentHealth, 100.0f);
        }

        //// �v���C���[�̃X�L���Q�[�W��ϓ�������
        //for (int i = 0; i < playerSkillBar.Length; i++)
        //{
        //    VariableBar(playerSkillBar[i], MAX_TIME, MAX_TIME - time);
        //}

        // �G�̗̑͂��Q�[�W�ŕϓ�������
        for (int i = 0; i < enemyHealthBar.Length; i++)
        {
            VariableBar(enemyHealthBar[i], enemyBear.status.m_health, enemyBear.status.m_healthMax);
        }

        //// �G�̃u���C�N�l�̗��܂���ϓ�������
        //for (int i = 0; i < enemyBreakBar.Length; i++)
        //{

        //}
    }

    // Image��Bar�̒l��ύX����
    public void VariableBar(Image barImage, float value, float maxValue)
    {
        // ��������
        barImage.GetComponent<Image>().fillAmount = value / maxValue;
    }

}
