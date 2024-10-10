using UnityEngine;

[CreateAssetMenu(fileName = "GreaterFortune", menuName = "ScriptableObject/Item/GreaterFortune")]
public class GreaterFortune : ItemBase
{
    protected override void Use()
    {
        UIManager.Instance.Get<UnitBuyUI>().UnitRankPercentage[UnitRank.Unique] += Value;
    }
}