/*
* @file UIManager.cs
* @brief �Q�[����UI���Ǘ�����N���X
* @author sein
* @date 2025/1/31
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static CommonModule;

// �o����ΓG�̍ő�̗͂�����UI��L�΂�
// �̗͂�����܂Ń��O�����
// Pooling�Ŏ�������

// ��Canvas��G�̐�������������͕̂`��R�X�g�������A���ׂɂȂ���

public class UIManager : SystemObject
{
    public static UIManager instance { get; private set; } = null;

    private readonly int INITIAL_COUNT = 1;

    [SerializeField] private GameObject enemyUIObject;
    [SerializeField] private GameObject canvasWorldSpace;

    public Camera mainCamera { get; private set; } = null;
    private Transform parent = null;

    private List<EnemyUI> enemyUIList = null;
    private List<GameObject> useObjectList = null;
    private Queue<GameObject> unuseObjectQueue = null;

    Vector3 viewportPos ;

    public override void Initialize()
    {
        instance = this;

        // Canvas��transform���擾
        parent = GameObject.Find("CanvasWorldSpace").transform;

        mainCamera = CameraManager.instance.selfCamera;

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
            }
        }

        for (int i = 0, max = enemyUIList.Count; i < max; i++)
        {
            if (IsEmpty(enemyUIList) != true)
                ;
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

    public void RemoveEnemyUI(int index)
    {
        enemyUIList[index].Teardown();

        unuseObjectQueue.Enqueue(useObjectList[index]);
        useObjectList.RemoveAt(index);
        enemyUIList.RemoveAt(index);
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
        
        // �g�p�O�̃Z�b�g�A�b�v 
        enemyUI.Setup(baseEnemy, UIObject);

        // �g�p�����X�g�ɒǉ�
        useObjectList.Add(UIObject);
        enemyUIList.Add(enemyUI);
    }
}
