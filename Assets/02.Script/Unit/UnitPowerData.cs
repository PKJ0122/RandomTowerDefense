using UnityEngine;

[CreateAssetMenu(fileName = "UnitPowerData", menuName = "ScriptableObject/UnitPowerData")]
public class UnitPowerData : ScriptableObject
{
    [SerializeField] public UnitKind unitKind;
    [SerializeField] public float[] unitPowerDatas = new float[System.Enum.GetValues(typeof(UnitRank)).Length];
}