using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    [SerializeField] private List<SystemObject> originSystemList = null; 
    private List<SystemObject> systemObjectList = null;

    private void Awake()
    {
        //systemObjectList = new List<SystemObject>();

        //foreach(SystemObject _originSystemList in originSystemList)
        //{
        //    if (_originSystemList == null) return;
        //    // ��������
        //    var systemObject = Instantiate(_originSystemList, transform.position, Quaternion.identity, transform);
        //    systemObjectList.Add(systemObject);
        //}

        //foreach(var systemObject in systemObjectList)
        //{
        //    // ���s����
        //    systemObject.Initialize();
        //}
    }


}
