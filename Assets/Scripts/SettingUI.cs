using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingUI : MonoBehaviour
{
    [SerializeField] private Slider _bgmVolume;
    [SerializeField] private Slider _sfxVolume;
    [SerializeField] private Toggle _bgmToggle;
    [SerializeField] private Toggle _sfxToggle;
    // Start is called before the first frame update
    void Start()
    {
        _bgmVolume.value = AudioManager.instance.bgmVolume;
        _sfxVolume.value = AudioManager.instance.sfxVolume;
        _bgmToggle.isOn = AudioManager.instance._bgmPlayState;
        _sfxToggle.isOn = AudioManager.instance._sfxPlayState;
    }

    public void Chk_Mute_BGM(bool mute)
    {
        AudioManager.instance.MuteBGM(mute);
    }

    public void Chk_Mute_SFX(bool mute)
    {
        AudioManager.instance.MuteSFX(mute);
    }

    public void ChangeBGMVolume()
    {
        AudioManager.instance.SetBackGroundVolume(_bgmVolume.value);
    }

    public void ChangeSFXVolume()
    {
        AudioManager.instance.SetSFXVolume(_sfxVolume.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
