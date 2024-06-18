using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitBuyInfoButtonUI : UIBase
{
    Button _unitBuyInfo;

    protected override void Awake()
    {
        base.Awake();
        _unitBuyInfo = transform.Find("Button - UnitBuyInfo").GetComponent<Button>();
        _unitBuyInfo.onClick.AddListener(() =>
        {
            UIManager.Instance.Get<UnitBuyInfoUI>().Show();
        });
    }
}
