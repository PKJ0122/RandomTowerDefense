using UnityEngine;

[CreateAssetMenu(fileName = "GreaterFortune", menuName = "ScriptableObject/Item/GreaterFortune")]
public class GreaterFortune : ItemBase
{
    protected override void Use()
    {
        UIManager.Instance.Get<UnitBuyUI>().UnitRankPercentage[UnitRank.Unique] += Value;
        float per = UIManager.Instance.Get<UnitBuyUI>().UnitRankPercentage[UnitRank.Unique];
        UIManager.Instance.Get<UnitBuyUI>().OnUnitBuySuccess += unit =>
        {
            if (unit.Rank != UnitRank.Unique) return;

            float num = Random.Range(0f, per);

            if (num <= Value)
            {
                Notice();
            }
        };
    }
}