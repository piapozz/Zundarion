using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : SystemObject
{
    public static FadeManager instance = null;

    /// <summary>�Ó]�p���e�N�X�`��</summary>
    private GameObject _fadeImage;
    private Material _fadeMaterial;

    public override void Initialize()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        if (this != instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        // �t�F�[�h�v���n�u�擾
        _fadeImage = Resources.Load<GameObject>("Prefabs/Fade/FadeImage");
        GameObject fade = Instantiate(_fadeImage, transform);
        _fadeMaterial = fade.GetComponentInChildren<Image>().material;
        _fadeMaterial.SetFloat("_Transition", 1);
    }

    /// <summary>
    /// �V�[���J�ڗp�R���[�`��
    /// </summary>
    /// <param name='scene'>�V�[����</param>
    /// <param name='interval'>�Ó]�ɂ����鎞��(�b)</param>
    public async UniTask TransScene(string scene, float interval)
    {
        //���񂾂�Â�
        float transition = 1;
        float time = 0;

        while (time < interval)
        {
            time += Time.deltaTime;
            transition = 1 - time / interval;
            _fadeMaterial.SetFloat("_Transition", transition);
            await UniTask.DelayFrame(1);
        }

        //�V�[���ؑ�
        SceneManager.LoadScene(scene);

        //���񂾂񖾂邭
        transition = 1;
        time = 0;
        while (time < interval)
        {
            time += Time.deltaTime;
            transition = time / interval;
            _fadeMaterial.SetFloat("_Transition", transition);
            await UniTask.DelayFrame(1);
        }

        await UniTask.DelayFrame(1);
    }

}
