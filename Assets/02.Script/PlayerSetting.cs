using System;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerSetting
{
    public float masterVolume = -10;
    public bool masterMute;
    public float bgmVolume = -10;
    public bool bgmMute;
    public float sfxVolume = -10;
    public bool sfxMute;
    public bool vibration = true;
    public int frame = 3;
}