using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMono<AudioManager>
{
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    [Header("音量控制")]
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float bgmVolume = 1f;

    [Header("开关控制")]
    public bool playBGM = true;
    public bool playSFX = true;
    private int bgmIndex;

    private void Awake()
    {
        UpdateAllVolumes();
    }

    private void Update()
    {
        UpdateAllVolumes();
        if (!playBGM)
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }
    }

    // 更新所有音频源的音量
    private void UpdateAllVolumes()
    {
        foreach (var source in sfx)
        {
            if (source != null)
                source.volume = masterVolume * sfxVolume;
        }

        foreach (var source in bgm)
        {
            if (source != null)
                source.volume = masterVolume * bgmVolume;
        }
    }

    // 全局音量控制属性
    public float MasterVolume
    {
        get { return masterVolume; }
        set
        {
            masterVolume = Mathf.Clamp(value, 0f, 1f);
            UpdateAllVolumes();
        }
    }

    // 音效音量控制属性
    public float SFXVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolume = Mathf.Clamp(value, 0f, 1f);
            UpdateAllVolumes();
        }
    }

    // BGM音量控制属性
    public float BGMVolume
    {
        get { return bgmVolume; }
        set
        {
            bgmVolume = Mathf.Clamp(value, 0f, 1f);
            UpdateAllVolumes();
        }
    }

    // 播放音效
    public void PlaySFX(int _SFXIndex)
    {
        if (!playSFX) return;
        if (_SFXIndex < sfx.Length && sfx[_SFXIndex] != null)
            sfx[_SFXIndex].Play();
    }

    // 停止单个音效
    public void StopSFX(int _SFXIndex)
    {
        if (_SFXIndex < sfx.Length && sfx[_SFXIndex] != null)
            sfx[_SFXIndex].Stop();
    }

    // 停止所有音效
    public void StopAllSFX()
    {
        foreach (var source in sfx)
        {
            if (source != null)
                source.Stop();
        }
    }

    // 播放BGM
    public void PlayBGM(int _BGMIndex)
    {
        bgmIndex = _BGMIndex;
        StopAllBGM();
        if (bgmIndex < bgm.Length && bgm[bgmIndex] != null)
            bgm[bgmIndex].Play();
    }

    // 停止所有BGM
    public void StopAllBGM()
    {
        foreach (var source in bgm)
        {
            if (source != null)
                source.Stop();
        }
    }

    // 开启所有音效
    public void EnableAllSFX()
    {
        playSFX = true;
    }

    // 关闭所有音效
    public void DisableAllSFX()
    {
        playSFX = false;
        StopAllSFX();
    }
}