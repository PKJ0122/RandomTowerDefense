using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitRankData", menuName = "ScriptableObject/UnitRankData")]
[Serializable]
public class UnitRankData : ScriptableObject
{
    public UnitRank unitRank;
    public string unitRankName;
    public Color unitRankColor;
}