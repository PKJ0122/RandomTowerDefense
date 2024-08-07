using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : SingletonMonoBase<SoundManager>
{
    PlayerSetting _playerSetting;

    AudioMixer _mixer;
    public AudioMixer Mixer
    {
        get
        {
            if (_mixer == null)
            {
                _mixer = Resources.Load<AudioMixer>("AudioMixer");
            }
            return _mixer;
        }
    }

    SoundDatas _soundDatas;

    List<AudioSource> _sources = new List<AudioSource>(16);


    protected override void Awake()
    {
        base.Awake();
        _playerSetting = PlayerSettingUI.PlayerSetting;
        _soundDatas = Resources.Load<SoundDatas>("SoundDatas");
    }

    public void Vibrate()
    {
        if (!_playerSetting.vibration) return;

        Handheld.Vibrate();
    }

    public void VolumeChange(string name, float value)
    {
        if (value <= -30)
        {
            value = -80;
        }

        Mixer.SetFloat(name, value);
    }

    public void PlaySound(BGM bgm)
    {
        foreach (AudioSource source in _sources)
        {
            if (!source.isPlaying)
            {
                source.clip = _soundDatas.bgms[(int)bgm];
                source.Play();
                source.outputAudioMixerGroup = Mixer.FindMatchingGroups("BGM")[0];
                source.loop = true;
                return;
            }
        }

        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        _sources.Add(audioSource);
        audioSource.clip = _soundDatas.bgms[(int)bgm];
        audioSource.Play();
        audioSource.outputAudioMixerGroup = Mixer.FindMatchingGroups("BGM")[0];
        audioSource.loop = true;
    }

    public void PlaySound(SFX sfx)
    {
        foreach (AudioSource source in _sources)
        {
            if (!source.isPlaying)
            {
                source.clip = _soundDatas.sfxs[(int)sfx];
                source.Play();
                source.outputAudioMixerGroup = Mixer.FindMatchingGroups("SFX")[0];
                source.loop = false;
                return;
            }
        }

        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        _sources.Add(audioSource);
        audioSource.clip = _soundDatas.sfxs[(int)sfx];
        audioSource.Play();
        audioSource.outputAudioMixerGroup = Mixer.FindMatchingGroups("SFX")[0];
        audioSource.loop = false;
    }
}
