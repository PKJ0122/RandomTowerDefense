using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FigureCollector", menuName = "ScriptableObject/Mission/FigureCollector")]
public class FigureCollector : MissionBase
{
    Dictionary<UnitKind,int> _unitChecker = new Dictionary<UnitKind,int>();

    public override void Init()
    {
        base.Init();
        for (int i = 0; i < System.Enum.GetValues(typeof(UnitKind)).Length; i++)
        {
            _unitChecker.TryAdd((UnitKind)i, 0);
        }
        UIManager.Instance.Get<UnitBuyUI>().OnUnitBuySuccess += unit =>
        {
            if (unit.Rank == UnitRank.Nomal)
            {
                unit.OnDisable += () =>
                {
                    if (--_unitChecker[unit.Kind] <= 0) Progress--;
                };

                if (_unitChecker[unit.Kind]++ == 0) Progress++;
            }
        };
    }
}
