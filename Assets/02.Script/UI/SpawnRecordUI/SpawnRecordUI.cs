using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRecordUI : UIBase
{
    Transform _location;
    SpawnRecordText _textPrefeb;


    protected override void Awake()
    {
        base.Awake();
        _location = transform.Find("Scroll View - SpawnRecord/Viewport/Content").GetComponent<Transform>();
        _textPrefeb = Resources.Load<SpawnRecordText>("GameObject - SpawnRecord");
        ObjectPoolManager.Instance.CreatePool("SpawnRecordText", _textPrefeb, 5);
    }

    void Start()
    {
        UIManager.Instance.Get<UnitBuyUI>().OnUnitBuySuccess += unit => CreateText(unit);
        UIManager.Instance.Get<UnitInfoUI>().OnUnitMix += unit => CreateText(unit);
    }

    void CreateText(UnitBase unit)
    {
        if ((int)unit.Rank < (int)UnitRank.Unique) return;

        SpawnRecordText obj = ObjectPoolManager.Instance.Get("SpawnRecordText")
                                                        .Get()
                                                        .GetComponent<SpawnRecordText>()
                                                        .SetText(unit);

        obj.transform.SetParent(_location,false);

        Handheld.Vibrate();
    }
}
