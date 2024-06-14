using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitDatas", menuName = "ScriptableObject/UnitDatas")]
[Serializable]
public class UnitDatas : ScriptableObject
{
    public UnitData[] unitDatas = new UnitData[Enum.GetValues(typeof(UnitKind)).Length];
    public UnitRankData[] unitRankColorDatas = new UnitRankData[Enum.GetValues(typeof(UnitRank)).Length];
}