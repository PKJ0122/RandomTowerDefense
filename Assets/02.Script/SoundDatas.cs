using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDatas",menuName = "ScriptableObject/SoundDatas")]
[Serializable]
public class SoundDatas : ScriptableObject
{
    public AudioClip[] bgms;
    public AudioClip[] sfxs;
}