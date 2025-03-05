using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    [SerializeField]
    private List<SystemObject> originSystemList = null;

    private List<SystemObject> systemList = null;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Start()
    {
        systemList = new List<SystemObject>();

        for (int i = 0, max = originSystemList.Count; i < max; i++)
        {
            SystemObject origin = originSystemList[i];
            if (origin == null) continue;

            SystemObject createObj = Instantiate(origin, transform);
            createObj.Initialize();

            systemList.Add(createObj);
        }
        AudioManager.instance.PlayBGM(BGM.TITLE);
    }

    public void ChangeScene(string name)
    {
        FadeManager.instance.TransScene(name, 1);
    }
}
