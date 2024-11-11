using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    static string nowScene = "";                    // シーンの名前を保持する

    public enum SceneName
    {
        SCENE_TITLE = 0,
        SCENE_MAIN,
        SCENE_RESULT,

        MAX
    }

    // シーンを変更
    void SceneChange(string sceneName,bool viewJingle)
    {
        // もしジングルを流すフラグがtrueだったら
        if(viewJingle)
        {
            // ジングルを流す


        }

        // シーンの名前を保存
        nowScene = sceneName;

        // シーンを変更
        SceneManager.LoadScene(sceneName);
    }
}
