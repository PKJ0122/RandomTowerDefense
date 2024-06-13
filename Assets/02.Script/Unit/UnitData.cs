using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObject/UnitData")]
[Serializable]
public class UnitData : ScriptableObject
{
    public UnitPowerData[] unitPowerDatas = new UnitPowerData[Enum.GetValues(typeof(UnitKind)).Length];
    public UnitRankColorData[] unitRankColorDatas = new UnitRankColorData[Enum.GetValues(typeof(UnitRank)).Length];
}
