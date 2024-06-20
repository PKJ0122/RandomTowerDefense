using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[Serializable]
public abstract class SkillBase : ScriptableObject
{
    public string skillname;
    [Multiline(5)]
    public string description;
    public Sprite skillImg;
    public int needMp;
    public float coefficient;

    public abstract void Skill(UnitBase caster);
}