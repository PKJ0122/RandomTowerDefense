using UnityEngine;

[CreateAssetMenu(fileName = "Fortunate", menuName = "ScriptableObject/Item/Fortunate")]
public class Fortunate : ItemBase
{
    protected override void Use()
    {
        UIManager.Instance.Get<UnitBuyUI>().UnitRankPercentage[UnitRank.Epic] += Value;
        float per = UIManager.Instance.Get<UnitBuyUI>().UnitRankPercentage[UnitRank.Epic];
        UIManager.Instance.Get<UnitBuyUI>().OnUnitBuySuccess += unit =>
        {
            if (unit.Rank != UnitRank.Epic) return;

            float num = Random.Range(0f, per);

            if (num <= Value)
            {
                Notice();
            }
        };
    }
}
