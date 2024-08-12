using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class UnitBuyInfoUI : UIBase
{
    TMP_Text _legendaryPercentage;
    TMP_Text _uniquePercentage;
    TMP_Text _epicPercentage;
    TMP_Text _rarePercentage;
    TMP_Text _nomalPercentage;
    Button _close;

    bool _isInit;

    protected override void Awake()
    {
        base.Awake();
        _legendaryPercentage = transform.Find("Panel/Panel/Text (TMP) - Legendary").GetComponent<TMP_Text>();
        _uniquePercentage = transform.Find("Panel/Panel/Text (TMP) - Unique").GetComponent<TMP_Text>();
        _epicPercentage = transform.Find("Panel/Panel/Text (TMP) - Epic").GetComponent<TMP_Text>();
        _rarePercentage = transform.Find("Panel/Panel/Text (TMP) - Rare").GetComponent<TMP_Text>();
        _nomalPercentage = transform.Find("Panel/Panel/Text (TMP) - Nomal").GetComponent<TMP_Text>();

        _close = transform.Find("Panel/Panel/Button - CloseButton").GetComponent<Button>();
        _close.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _close.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    public override void Show()
    {
        if (!_isInit) Init();

        base.Show();
    }

    void Init()
    {
        _isInit = true;
        Dictionary<UnitRank, float> unitPercentage = UIManager.Instance.Get<UnitBuyUI>().UnitRankPercentage;

        _legendaryPercentage.text = $"{unitPercentage[UnitRank.Legendary]}%";
        _uniquePercentage.text = $"{unitPercentage[UnitRank.Unique]}%";
        _epicPercentage.text = $"{unitPercentage[UnitRank.Epic]}%";
        _rarePercentage.text = $"{unitPercentage[UnitRank.Rare]}%";
        float nomalPercentage = 100f - unitPercentage[UnitRank.Legendary]
                                     - unitPercentage[UnitRank.Unique]
                                     - unitPercentage[UnitRank.Epic]
                                     - unitPercentage[UnitRank.Rare];
        _nomalPercentage.text = $"{nomalPercentage}%";
    }
}
