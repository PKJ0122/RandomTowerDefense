using UnityEngine;

[CreateAssetMenu(fileName = "Fortunate", menuName = "ScriptableObject/Item/Fortunate")]
public class Fortunate : ItemBase
{
    protected override void Use()
    {
        UIManager.Instance.Get<UnitBuyUI>().UnitRankPercentage[UnitRank.Epic] += Value;
    }
}
