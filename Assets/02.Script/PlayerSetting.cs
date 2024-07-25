using System;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerSetting
{
    public float masterVolume;
    public bool masterMute;
    public float bgmVolume;
    public bool bgmMute;
    public float sfxVolume;
    public bool sfxMute;
    public bool vibration = true;
    public int frame = 3;
}