using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitName", menuName = "ScriptableObject/UnitData")]
[Serializable]
public class UnitData : ScriptableObject
{
    public UnitKind unitKind;
    public UnitBase unitObject;
    public string unitName;
    public float[] unitPowerDatas = new float[Enum.GetValues(typeof(UnitRank)).Length];
}