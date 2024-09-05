using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#TitleBGM")]
    public AudioClip _titleBGM;

    [Header("#LobbyBGM")]
    public AudioClip _lobbyBGM;

    [Header("#BGM")]
    public AudioClip InGameBgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public AudioClip[] _uiSfx;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public bool _bgmPlayState;
    public bool _sfxPlayState;

    public AudioHighPassFilter _highAudioFilter;

    public enum skillSfx { Slash = 0, Arrow, Magic }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void TitleBgm(bool isPlay)
    {
        if (bgmPlayer.isPlaying)
            bgmPlayer.Stop();

        bgmPlayer.clip = _titleBGM;

        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void LobbyBgm(bool isPlay)
    {
        if (bgmPlayer.isPlaying)
            bgmPlayer.Stop();

        bgmPlayer.clip = _lobbyBGM;

        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void PlayInGameBgm(bool isPlay)
    {
        if (bgmPlayer.isPlaying)
            bgmPlayer.Stop();

        bgmPlayer.clip = InGameBgmClip;

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

    public AudioClip SetClip(int _ClipNumber)
    {
        return _uiSfx[_ClipNumber];
    }

    public void SetBackGroundVolume(float _volume)
    {
        bgmPlayer.volume = _volume;
        bgmVolume = _volume;
    }

    public void MuteBGM(bool isPlay)
    {
        bgmPlayer.mute = isPlay;
        _bgmPlayState = isPlay;
    }

    public void MuteSFX(bool isPlay)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index].mute = isPlay;
        }

        _sfxPlayState = isPlay;
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;

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

    private void Update()
    {
        if (_highAudioFilter == null)
            _highAudioFilter = FindObjectOfType<AudioHighPassFilter>();
    }
}
