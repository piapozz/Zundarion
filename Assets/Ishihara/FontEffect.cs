using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontEffect : MonoBehaviour
{
    [SerializeReference, SubclassSelector(true)]
    FadeInFont fadeInFont;

    [SerializeReference, SubclassSelector(true)]
    FadeOutFont fadeOutFont;

    [SerializeField]
    private bool _isDestory = false;

    public async void Execution()
    {
        await fadeInFont.FadeIn();
        await fadeOutFont.FadeOut();

        if(_isDestory) gameObject.SetActive(false);
    }

    public async void FadeIn(float value = 1)
    {
        await fadeInFont.FadeIn(value);
    }

    public async void FadeOut(float value = 1)
    {
        await fadeOutFont.FadeOut(value);
    }
}
