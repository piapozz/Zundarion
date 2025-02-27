/*
 * @file UIManager.cs
 * @brief �Q�[����UI���Ǘ�����N���X
 * @author sein
 * @date 2025/1/31
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static CommonModule;
using static UnityEngine.EventSystems.EventTrigger;

// �o����ΓG�̍ő�̗͂�����UI��L�΂�
// �̗͂�����܂Ń��O�����
// Pooling�Ŏ�������

// ��Canvas��G�̐�������������͕̂`��R�X�g�������A���ׂɂȂ���

public class UIManager : SystemObject
{
    public static UIManager instance { get; private set; } = null;

    private readonly int INITIAL_COUNT = 1;
    public static readonly int POOL_COUNT = 100;

    [SerializeField] private GameObject damageEffect;

    [SerializeField] private GameObject playerUIObject;
    [SerializeField] private GameObject enemyUIObject;
    [SerializeField] private GameObject canvasWorldSpace;
    [SerializeField] private GameObject canvasOverlay;
    [SerializeField] private LayerMask obstacleLayer;

    public Camera mainCamera { get; private set; } = null;
    private Transform parent = null;
    private Transform parentOverlay = null;

    private List<EnemyUI> enemyUIList = null;
    private List<GameObject> useObjectList = null;
    private Queue<GameObject> unuseObjectQueue = null;
    List<GameObject> _damageEffectList = null;

    Vector3 viewportPos;

    public override void Initialize()
    {
        instance = this;

        GameObject worldSpace = Instantiate(canvasWorldSpace, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        GameObject spaceOverlay = Instantiate(canvasOverlay, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

        // Canvas��transform���擾
        mainCamera = CameraManager.instance.selfCamera;

        parent = worldSpace.transform;
        parentOverlay = spaceOverlay.transform;

        AddPlayerUI(CharacterManager.instance.characterList[0].GetComponent<BasePlayer>());

        // �S�Ă��󂾂����炠�炩���ߐ�������
        if (useObjectList == null && unuseObjectQueue == null)
        {
            useObjectList = new List<GameObject>();
            unuseObjectQueue = new Queue<GameObject>();
            enemyUIList = new List<EnemyUI>();

            for (int i = 0, max = INITIAL_COUNT; i < max; i++)
            {
                unuseObjectQueue.Enqueue(Instantiate(enemyUIObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, parent));
            }
        }
        _damageEffectList = new List<GameObject>(POOL_COUNT);
        // �v�[��
        for (int i = 0; i < POOL_COUNT; i++)
        {
            var effect = Instantiate(damageEffect, new Vector3(0, 0, 0), Quaternion.identity, spaceOverlay.transform);
            effect.SetActive(false);
            _damageEffectList.Add(effect);
        }
    }

    public override void Proc()
    {
        // �s�K�v��UI���\���ɂ���
        for (int i = 0, max = enemyUIList.Count; i < max; i++)
        {
            if (enemyUIList[i] == null) break;

            Vector3 enemyPosition = enemyUIList[i].enemyPosition;

            viewportPos = mainCamera.WorldToViewportPoint(enemyPosition + Vector3.up * 2f);

            // ��ʂɈڂ�����\���ɂ���
            if (viewportPos.z > 0 && viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1)
            {
                enemyUIList[i].SetActive(true);
            }
            // ��ʊO�ɊO�ꂽ���\���ɂ���
            else
            {
                enemyUIList[i].SetActive(false);
                return;
            }
        }

        // �I�u�W�F�N�g��UI����\���ɂȂ�
        for (int i = 0, max = enemyUIList.Count; i < max; i++)
        {

            Vector3 enemyPosition = enemyUIList[i].enemyPosition + Vector3.up * 1f;

            Vector3 direction = (enemyPosition - mainCamera.transform.position).normalized;
            float distance = Vector3.Distance(mainCamera.transform.position, enemyPosition);

            bool isObstructed = Physics.Raycast(mainCamera.transform.position, direction, distance, obstacleLayer);

            // Debug.DrawRay(mainCamera.transform.position, direction * distance, isObstructed ? Color.red : Color.green, 0.1f);

            enemyUIList[i].SetActive(!isObstructed);
        }

        for (int i = 0, max = enemyUIList.Count; i < max; i++)
        {
            if (IsEmpty(enemyUIList) != true)
                
            if (enemyUIList[i].health <= 0)
            {
                RemoveEnemyUI(i);
                max -= 1;
            }
        }
    }

    public void AddEnemyUI(BaseEnemy baseEnemy)
    {
        GenerateEnemyUI(baseEnemy);
    }

    public void AddPlayerUI(BasePlayer basePlayer)
    {
        GeneratePlayerUI(basePlayer);
    }

    public void RemoveEnemyUI(int index)
    {
        enemyUIList[index].Teardown();

        unuseObjectQueue.Enqueue(useObjectList[index]);
        useObjectList.RemoveAt(index);
        enemyUIList.RemoveAt(index);
    }

    public void GenerateDamageEffect(Vector3 position, int damage, Color color)
    {
        position = Camera.main.WorldToScreenPoint(position);
        int activeNumber = -1;
        for (int i = 0; i < POOL_COUNT; i++)
        {
            if (_damageEffectList[i] == null || !_damageEffectList[i].activeSelf)
            {
                activeNumber = i;
                _damageEffectList[activeNumber].SetActive(true);
                break;
            }
        }

        if (activeNumber < 0) return;

        // �ʒu�����炷
        float dir = UnityEngine.Random.Range(0,360);
        float length = UnityEngine.Random.Range(100,150);

        SetPosition(_damageEffectList[activeNumber], dir, length, position);

        // �_���[�W�̒l��ݒ肷��
        var text =_damageEffectList[activeNumber].GetComponent<TextMeshProUGUI>();
        if (text == null) return;

        text.text = string.Format("{0}!!", damage);

        // �F��ݒ�
        text.color = color;
        text.faceColor = Color.white;

        var damageEffect = _damageEffectList[activeNumber].GetComponent<DamageFont>();
        damageEffect.Execution();

    }

    private void SetPosition(GameObject effect, float dir, float length, Vector3 position)
    {
        float angleInRadians = dir * Mathf.Deg2Rad;
        Vector3 offset;
        offset.x = length * Mathf.Cos(angleInRadians);
        offset.y = length * Mathf.Sin(angleInRadians);
        offset.z = 0;

        effect.transform.position = offset + position;
    }
    private void GeneratePlayerUI(BasePlayer basePlayer)
    {
        PlayerUI playerUI = null;
        GameObject UIObject = Instantiate(playerUIObject, new Vector3(-150.0f, 80.0f, 0.0f), Quaternion.identity, parentOverlay);

        // �N���X���擾
        playerUI = UIObject.GetComponent<PlayerUI>();
        playerUI.Setup(basePlayer);

        RectTransform rectTransform = UIObject.GetComponent<RectTransform>();

        // ����ɔz�u���邽�߂̐ݒ�
        rectTransform.anchorMin = new Vector2(0, 1);  // ����
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);      // ����s�{�b�g
        rectTransform.anchoredPosition = new Vector2(50, -30); // ���ォ�班���I�t�Z�b�g
    }

    /// <summary>
    /// �G��UI�̌��т����s��
    /// </summary>
    /// <param name="baseEnemy"></param>
    private void GenerateEnemyUI(BaseEnemy baseEnemy)
    {
        GameObject UIObject = null;
        EnemyUI enemyUI = null;

        // UI�I�u�W�F�N�g���ė��p�܂��͐�������
        if (IsEmpty(unuseObjectQueue) != true)
        {
            UIObject = unuseObjectQueue.Dequeue();
        }
        else
        {
            UIObject = Instantiate(enemyUIObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, parent);
        }

        // �N���X���擾
        enemyUI = UIObject.GetComponent<EnemyUI>();
        enemyUI.Setup(baseEnemy, UIObject);

        // �g�p�����X�g�ɒǉ�
        useObjectList.Add(UIObject);
        enemyUIList.Add(enemyUI);
    }
}
