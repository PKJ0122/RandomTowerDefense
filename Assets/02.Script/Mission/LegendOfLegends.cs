using UnityEngine;

[CreateAssetMenu(fileName = "LegendOfLegends", menuName = "ScriptableObject/Mission/LegendOfLegends")]
public class LegendOfLegends : MissionBase
{
    public override void Init()
    {
        base.Init();
        UIManager.Instance.Get<UnitInfoUI>().OnUnitMix += unit =>
        {
            if (unit.Rank == UnitRank.Legendary) Progress++;
        };
    }
}