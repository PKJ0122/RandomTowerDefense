using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(fileName = "UnitName", menuName = "ScriptableObject/UnitData")]
[Serializable]
public class UnitData : ScriptableObject
{
    public UnitKind unitKind;
    public UnitBase unitObject;
    public string unitName;
    public float[] unitPowerDatas = new float[Enum.GetValues(typeof(UnitRank)).Length];
    public SkillBase skill;
    public Sprite unitImg;
    public BeyondBase beyondObject;
}