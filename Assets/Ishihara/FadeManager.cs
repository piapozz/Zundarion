using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : SystemObject
{
    public static FadeManager instance = null;

    /// <summary>�Ó]�p���e�N�X�`��</summary>
    private GameObject _fadeImage;
    private Material _fadeMaterial;
    private Canvas _canvas;

    public override void Initialize()
    {
        if (instance == null)
        {
            instance = this;
        }
        // �t�F�[�h�v���n�u�擾
        _fadeImage = Resources.Load<GameObject>("Prefabs/Fade/FadeImage");
        GameObject fade = Instantiate(_fadeImage, transform);

        _canvas = fade.GetComponent<Canvas>();
        _fadeMaterial = fade.GetComponentInChildren<Image>().material;
        _fadeMaterial.SetFloat("_Transition", 1);
        _canvas.enabled = false; // �L�����o�X���\��
    }

    /// <summary>
    /// �V�[���J�ڗp�R���[�`��
    /// </summary>
    /// <param name='scene'>�V�[����</param>
    /// <param name='interval'>�Ó]�ɂ����鎞��(�b)</param>
    public async UniTask TransScene(string scene, float interval)
    {
        _canvas.enabled = true; // �L�����o�X��\��

        // ���񂾂�Â�
        float transition = 0;
        float time = 0;

        while (time < interval)
        {
            time += Time.deltaTime;
            transition = Mathf.Lerp(1, 0, time / interval);
            _fadeMaterial.SetFloat("_Transition", transition);
            await UniTask.Yield();
        }

        // �V�[���ؑցi�񓯊������j
        await SceneManager.LoadSceneAsync(scene);

        // ���񂾂񖾂邭
        transition = 1;
        time = 0;

        while (time < interval)
        {
            time += Time.deltaTime;
            transition = Mathf.Lerp(0, 1, time / interval);
            _fadeMaterial.SetFloat("_Transition", transition);
            await UniTask.Yield();
        }

        _canvas.enabled = false; // �L�����o�X���\��
    }
}
