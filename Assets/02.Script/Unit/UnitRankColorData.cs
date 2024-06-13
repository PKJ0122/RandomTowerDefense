using UnityEngine;

[CreateAssetMenu(fileName = "UnitRankColorData", menuName = "ScriptableObject/UnitRankColorData")]
public class UnitRankColorData : ScriptableObject
{
    [SerializeField] public UnitRank unitRank;
    [SerializeField] public Color unitRankColorData;
}