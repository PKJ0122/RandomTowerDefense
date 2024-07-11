using System;
using TMPro;

public class GoodsUI : UIBase
{
    TMP_Text _gold;
    TMP_Text _diamond;

    Action<int> OnGoldChangeHandler;
    Action<int> OnDiamondChangeHandler;


    protected override void Awake()
    {
        base.Awake();
        _gold = transform.Find("Panel - Gold/Text (TMP) - Gold").GetComponent<TMP_Text>();
        _diamond = transform.Find("Panel - Diamond/Text (TMP) - Diamond").GetComponent<TMP_Text>();
        _gold.text = PlayerData.Instance.Gold.ToString("N0");
        _diamond.text = PlayerData.Instance.Diamond.ToString("N0");
        OnGoldChangeHandler += value => _gold.text = value.ToString("N0");
        OnDiamondChangeHandler += value => _diamond.text = value.ToString("N0");
        PlayerData.OnGoldChange += OnGoldChangeHandler;
        PlayerData.OnDiamondChange += OnDiamondChangeHandler;
    }

    public void OnDestroy()
    {
        PlayerData.OnGoldChange -= OnGoldChangeHandler;
        PlayerData.OnDiamondChange -= OnDiamondChangeHandler;
    }
}