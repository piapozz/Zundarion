using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.VolumeComponent;

public class AudioManager : MonoBehaviour
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

    [SerializeField] GameObject BGMSourceOrigin;
    [SerializeField] GameObject SESourceOrigin;

    public static AudioManager instance = null;

    private List<AudioSource> BGMSourceList;
    private List<AudioSource> SESourceList;

    private List<AudioClip> BGMClip = new List<AudioClip>();
    private List<AudioClip> SEClip = new List<AudioClip>();

    void Awake()
    {
        instance = this;

        BGMSourceList = new List<AudioSource>();
        SESourceList = new List<AudioSource>();

        // BGMのclipを取得する
        BGMClip.Add(Resources.Load<AudioClip>("Sounds/BGM/SHOUTING_ON_THE_EDGE"));
        BGMClip.Add(Resources.Load<AudioClip>("Sounds/BGM/take_me_high"));
        BGMClip.Add(Resources.Load<AudioClip>("Sounds/BGM/オール・オール・グリーン・サイコ"));

        // SEのclipを取得する
        SEClip.Add(Resources.Load<AudioClip>("enter"));
        SEClip.Add(Resources.Load<AudioClip>("select"));
        SEClip.Add(Resources.Load<AudioClip>("return"));
        SEClip.Add(Resources.Load<AudioClip>("clear"));
        SEClip.Add(Resources.Load<AudioClip>("gameover"));
        SEClip.Add(Resources.Load<AudioClip>("juingle"));
        SEClip.Add(Resources.Load<AudioClip>("uiDisplay"));
    }

    private void Update()
    {

    }

    // BGMを流す関数
    public void PlayBGM(BGM num)
    {
        AudioSource source = GetUnusedBGMSource(BGMSourceList);

        source.clip = BGMClip[(int)num];

        source.Play();
    }

    // SEを鳴らす関数
    public void PlaySE(SE num, float volume)
    {
        AudioSource source = GetUnusedSESource(SESourceList);

        source.PlayOneShot(SEClip[(int)num], volume);
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
            source.Add(Instantiate(BGMSourceOrigin, transform).GetComponent<AudioSource>());
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
            source.Add(Instantiate(SESourceOrigin, transform).GetComponent<AudioSource>());
            number = source.Count - 1;
        }

        return source[number];
    }

}
