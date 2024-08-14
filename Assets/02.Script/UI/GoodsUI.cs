using TMPro;

public class GoodsUI : UIBase
{
    TMP_Text _gold;
    TMP_Text _diamond;


    protected override void Awake()
    {
        base.Awake();
        _gold = transform.Find("Panel - Gold/Text (TMP) - Gold").GetComponent<TMP_Text>();
        _diamond = transform.Find("Panel - Diamond/Text (TMP) - Diamond").GetComponent<TMP_Text>();
        _gold.text = PlayerData.Instance.Gold.ToString("N0");
        _diamond.text = PlayerData.Instance.Diamond.ToString("N0");
        PlayerData.Instance.OnGoldChange += value => _gold.text = value.ToString("N0");
        PlayerData.Instance.OnDiamondChange += value => _diamond.text = value.ToString("N0");
    }
}