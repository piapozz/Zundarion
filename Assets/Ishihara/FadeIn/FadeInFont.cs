using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FadeInFont
{
    public abstract UniTask FadeIn(float value = 1);
}
