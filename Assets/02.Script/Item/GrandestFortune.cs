using UnityEngine;

[CreateAssetMenu(fileName = "GrandestFortune", menuName = "ScriptableObject/Item/GrandestFortune")]
public class GrandestFortune : ItemBase
{
    protected override void Use()
    {
        UIManager.Instance.Get<UnitBuyUI>().UnitRankPercentage[UnitRank.Legendary] += Value;
    }
}
