using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : SingletonMonoBase<SoundManager>
{
    const int MIN_CHANNEL_COUNT = 1;
    const int MAX_CHANNEL_CONUT = 16;

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

    int _currentChannel;



    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        _currentChannel = MIN_CHANNEL_COUNT;
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
        if (_sources.Count == 0)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            _sources.Add(audioSource);
        }

        AudioSource source = _sources[0];
        source.clip = _soundDatas.bgms[(int)bgm];
        source.Play();
        source.outputAudioMixerGroup = Mixer.FindMatchingGroups("BGM")[0];
        source.loop = true;
    }

    public void PlaySound(SFX sfx)
    {
        for (int i = _sources.Count; i <= _currentChannel; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            _sources.Add(audioSource);
        }

        AudioSource source = _sources[_currentChannel];
        source.clip = _soundDatas.sfxs[(int)sfx];
        source.Play();
        source.outputAudioMixerGroup = Mixer.FindMatchingGroups("SFX")[0];
        source.loop = false;

        if (++_currentChannel >= MAX_CHANNEL_CONUT)
        {
            _currentChannel = MIN_CHANNEL_COUNT;
        }
    }
}
