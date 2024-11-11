using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    static string nowScene = "";                    // �V�[���̖��O��ێ�����

    public enum SceneName
    {
        SCENE_TITLE = 0,
        SCENE_MAIN,
        SCENE_RESULT,

        MAX
    }

    // �V�[����ύX
    void SceneChange(string sceneName,bool viewJingle)
    {
        // �����W���O���𗬂��t���O��true��������
        if(viewJingle)
        {
            // �W���O���𗬂�


        }

        // �V�[���̖��O��ۑ�
        nowScene = sceneName;

        // �V�[����ύX
        SceneManager.LoadScene(sceneName);
    }
}
