using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMono<AudioManager>
{
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBGM;
    private int bgmIndex;

    private void Update()
    {
        if (!playBGM)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying) 
                PlayBGM(bgmIndex);
        }
    }

    public void PlaySFX(int _SFXIndex)
    {
        if (_SFXIndex < sfx.Length) 
            sfx[_SFXIndex].Play();
    }

    public void StopSFX(int _SFXIndex) => sfx[_SFXIndex].Stop();

    public void PlayBGM(int _BGMIndex)
    {
        bgmIndex = _BGMIndex;
        StopAllBGM();
        if (bgmIndex < bgm.Length)
            bgm[bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
