using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeOutToTop : FadeOutFont
{
    [SerializeField]
    private TMP_Text _textMeshPro;


    [SerializeField]
    private float _fadeOutTime = 2;
    [SerializeField]
    private float _fadeOutSpeed = 1;

    public async UniTask FadeOut()
    {
        Move();
    }

    private async UniTask Move()
    {
        float elapsedTime = 0.0f;

        while (_textMeshPro != null && elapsedTime < _fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            _textMeshPro.transform.position += new Vector3(0, _fadeOutSpeed, 0);
            _textMeshPro.alpha = Mathf.Lerp(1, 0, elapsedTime / _fadeOutTime);
            await UniTask.DelayFrame(1);
        }
    }
}
