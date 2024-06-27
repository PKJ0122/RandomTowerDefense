using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FigureCollector", menuName = "ScriptableObject/Mission/FigureCollector")]
public class FigureCollector : MissionBase
{
    Dictionary<UnitKind,int> unitChecker = new Dictionary<UnitKind,int>();

    public override void Init()
    {
        base.Init();
        for (int i = 0; i < System.Enum.GetValues(typeof(UnitKind)).Length; i++)
        {
            unitChecker.Add((UnitKind)i, 0);
        }
        UIManager.Instance.Get<UnitBuyUI>().onUnitBuySuccess += unit =>
        {
            if (unit.Rank == UnitRank.Nomal)
            {
                unit.onDisable += () =>
                {
                    if (--unitChecker[unit.Kind] <= 0) Progress--;
                };

                if (unitChecker[unit.Kind]++ == 0) Progress++;
            }
        };
    }
}
