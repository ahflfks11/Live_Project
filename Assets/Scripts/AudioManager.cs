using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public AudioHighPassFilter _highAudioFilter;

    public enum skillSfx { Slash = 0, Arrow, Magic }

    private void Awake()
    {
        instance = this;
        Init();
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void OFFSFX()
    {
        _highAudioFilter.enabled = true;
    }

    public void ONSFX()
    {
        _highAudioFilter.enabled = false;
    }

    public void SetSFXVolume(float _volume)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index].volume = _volume;
        }

        sfxVolume = _volume;
    }

    public void SetBackGroundVolume(float _volume)
    {
        bgmPlayer.volume = _volume;
        bgmVolume = _volume;
    }

    public void MuteBGM(bool isPlay)
    {
        bgmPlayer.mute = isPlay;
    }

    public void MuteSFX(bool isPlay)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index].mute = isPlay;
        }
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlaySkillSfx(skillSfx sfx)
    {
        for(int index = 0; index<sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;
            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }    
}
