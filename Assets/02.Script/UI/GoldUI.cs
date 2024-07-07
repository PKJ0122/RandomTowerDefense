using TMPro;

public class GoldUI : UIBase
{
    TMP_Text _gold;

    protected override void Awake()
    {
        base.Awake();
        _gold = transform.Find("Panel/Text (TMP) - Gold").GetComponent<TMP_Text>();
        GameManager.Instance.OnGoldChange += value =>
        {
            _gold.text = value.ToString();
        };
    }
}
