using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 画像を管理
    [SerializeField] Image[] playerHealthBar;
    [SerializeField] Image[] enemyHealthBar;
    [SerializeField] Image[] playerSkillBar;
    [SerializeField] Image[] enemyBreakBar;

    // PlayerManager
    PlayerManager playerManager;
 
    // キャラクター
    BasePlayer zundaObject;
    BasePlayer zunkoObject;
    EnemyBase enemyBear;

    float time;

    const float MAX_TIME = 100.0f;

    int[] teamMember;

    // プレイヤーのステータス
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
        // PlayerManagerを取得
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        // Playerを取得
        zundaObject = GameObject.Find("Zundamon").GetComponent<Zunda>();
        zunkoObject = GameObject.Find("Zunko").GetComponent<Zunda>();

        // 敵を取得
        enemyBear = GameObject.Find("Bear").GetComponent<EnemyBase>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        // プレイヤーの体力をゲージで変動させる
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

        //// プレイヤーのスキルゲージを変動させる
        //for (int i = 0; i < playerSkillBar.Length; i++)
        //{
        //    VariableBar(playerSkillBar[i], MAX_TIME, MAX_TIME - time);
        //}

        // 敵の体力をゲージで変動させる
        for (int i = 0; i < enemyHealthBar.Length; i++)
        {
            VariableBar(enemyHealthBar[i], enemyBear.status.m_health, enemyBear.status.m_healthMax);
        }

        //// 敵のブレイク値の溜まり具合を変動させる
        //for (int i = 0; i < enemyBreakBar.Length; i++)
        //{

        //}
    }

    // ImageのBarの値を変更する
    public void VariableBar(Image barImage, float value, float maxValue)
    {
        // 割合を代入
        barImage.GetComponent<Image>().fillAmount = value / maxValue;
    }

}
