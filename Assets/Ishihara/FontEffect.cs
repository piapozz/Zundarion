using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FontEffect : MonoBehaviour
{
    [SerializeReference]
    FadeInFont fadeInFont;

    [SerializeReference]
    FadeOutFont fadeOutFont;

    public async void Execution()
    {
        await fadeInFont.FadeIn();
        await fadeOutFont.FadeOut();

        gameObject.SetActive(false);
    }
}
