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

public class UIManager : SystemObject
{
    public static UIManager instance { get; private set; } = null;

    private readonly int INITIAL_COUNT = 3;

    [SerializeField] private GameObject enemyUIObject;
    [SerializeField] private GameObject canvasWorldSpace;

    private Camera mainCamera = null;
    private Transform parent = null;

    private List<EnemyUI> enemyUIList = null;
    private List<GameObject> useObjectList = null;
    private List<GameObject> unuseObjectList = null;

    Vector3 viewportPos;

    public override void Initialize()
    {
        instance = this;

        // Canvas��transform���擾
        parent = canvasWorldSpace.transform;

        mainCamera = CameraManager.instance.selfCamera;

        // �S�Ă��󂾂����炠�炩���ߐ�������
        if (useObjectList == null && unuseObjectList == null)
        {
            useObjectList = new List<GameObject>();
            unuseObjectList = new List<GameObject>();
            enemyUIList = new List<EnemyUI>();

            for (int i = 0, max = INITIAL_COUNT; i < max; i++)
            {
                unuseObjectList.Add(null);
                // unuseObjectList.Add(Instantiate(enemyUIObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, parent));
            }
        }
    }

    private void Update()
    {
        // �s�K�v��UI���\���ɂ���
        for (int i = 0, max = enemyUIList.Count; i < max; i++)
        {
            Debug.Log(i);
            if (enemyUIList[i] == null) break;

            viewportPos = mainCamera.WorldToViewportPoint(enemyUIList[i].baseEnemy.position + Vector3.up * 2f);

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
    }

    public void AddEnemyUI(BaseEnemy baseEnemy)
    {
        GenerateEnemyUI(baseEnemy);

    }

    public void RemoveEnemyUI(BaseEnemy baseEnemy)
    {

    }

    /// <summary>
    /// �G��UI�̌��т����s��
    /// </summary>
    /// <param name="baseEnemy"></param>
    private void GenerateEnemyUI(BaseEnemy baseEnemy)
    {
        Debug.Log("UI����");

        GameObject UIObject = null;
        EnemyUI enemyUI = null;

        // UI�I�u�W�F�N�g���ė��p�܂��͐�������
        if (unuseObjectList[0] != null)
        {
            UIObject = unuseObjectList[0];
            unuseObjectList.RemoveAt(0);
            Debug.Log("���g�p�Ȃ��̂��g�p����");
        }
        else UIObject = Instantiate(enemyUIObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, parent);

        // �N���X���擾
        enemyUI = UIObject.GetComponent<EnemyUI>();
        
        // �g�p�O�̃Z�b�g�A�b�v 
        enemyUI.Setup(baseEnemy, UIObject);

        // �g�p�����X�g�ɒǉ�
        useObjectList.Add(UIObject);
        enemyUIList.Add(enemyUI);
    }
}
