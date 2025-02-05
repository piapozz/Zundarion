using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    [SerializeField] List<SystemObject> originSystemList = null; 
    private List<SystemObject> systemObjectList = null;

    private void Awake()
    {
        for (int i = 0, max = originSystemList.Count; i < max; i++)
        {
            // ��������
            var systemObject = Instantiate(originSystemList[i], transform.position, Quaternion.identity, transform);
            systemObjectList.Add(systemObject);

            // ���s����
            systemObjectList[i].Initialize();
        }
    }


}
