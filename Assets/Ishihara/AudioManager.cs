using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SystemObject
{
    public enum BGM
    {
        TITLE = 0,
        MAIN,
        OTHER,

        MAX
    }

    public enum SE
    {
        ENTER = 0,
        SELECT,
        RETURN,
        CLEAR,
        GAMEOVER,
        JINGLE,
        UI_DISPLAY,

        MAX
    }

    AudioMixer audioMixer;

    [SerializeField]
    private GameObject _bgmSourceOrigin;

    [SerializeField]
    private GameObject _seSourceOrigin;

    [SerializeField]
    private BGMClip _bgmClip;

    public static AudioManager instance = null;

    private List<AudioSource> BGMSourceList;
    private List<AudioSource> SESourceList;

    public override void Initialize()
    {
        instance = this;

        BGMSourceList = new List<AudioSource>();
        SESourceList = new List<AudioSource>();
    }

    // BGMを流す関数
    public void PlayBGM(BGM num)
    {
        AudioSource source = GetUnusedBGMSource(BGMSourceList);

        source.clip = _bgmClip.bgmClips[(int)num];

        source.Play();
    }

    // SEを鳴らす関数
    public void PlaySE(SE num, float volume)
    {
        AudioSource source = GetUnusedSESource(SESourceList);

        source.PlayOneShot(_bgmClip.bgmClips[(int)num], volume);
    }

    // 未使用のソースを取得する
    private AudioSource GetUnusedBGMSource(List<AudioSource> source)
    {
        int number = -1;

        for (int i = 0, max = source.Count; i < max; i++)
        {
            if (source[i].time > 0)
            {
                continue;
            }

            number = i;
            break;
        }

        if(number == -1)
        {
            source.Add(Instantiate(_bgmSourceOrigin, transform).GetComponent<AudioSource>());
            number = source.Count - 1;
        }

        return source[number];
    }

    // 未使用のソースを取得する
    private AudioSource GetUnusedSESource(List<AudioSource> source)
    {
        int number = -1;

        for (int i = 0, max = source.Count; i < max; i++)
        {
            if (source[i].time > 0)
            {
                continue;
            }

            number = i;
            break;
        }

        if (number == -1)
        {
            source.Add(Instantiate(_seSourceOrigin, transform).GetComponent<AudioSource>());
            number = source.Count - 1;
        }

        return source[number];
    }

}
