using UnityEngine;

[CreateAssetMenu(fileName = "GrandestFortune", menuName = "ScriptableObject/Item/GrandestFortune")]
public class GrandestFortune : ItemBase
{
    protected override void Use()
    {
        UIManager.Instance.Get<UnitBuyUI>().UnitRankPercentage[UnitRank.Legendary] += Value;
        float per = UIManager.Instance.Get<UnitBuyUI>().UnitRankPercentage[UnitRank.Legendary];
        UnitFactory.Instance.OnUnitCreat += unit =>
        {
            if (unit.Rank != UnitRank.Legendary) return;

            float num = Random.Range(0f, per);

            if (num <= Value)
            {
                Notice();
            }
        };
    }
}
