using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : UIBase
{
    TMP_Text _gold;

    protected override void Awake()
    {
        base.Awake();
        _gold = transform.Find("Panel/Text (TMP) - Gold").GetComponent<TMP_Text>();
        GameManager.Instance.onGoldChange += value =>
        {
            _gold.text = value.ToString();
        };
    }
}
