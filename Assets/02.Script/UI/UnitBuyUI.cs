using System;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// ���� ���� UI / ����ó��
/// </summary>
public class UnitBuyUI : UIBase
{
    const int WEIGHT = 2;

    Button _unitBuy;
    TMP_Text _price;

    int Price
    {
        get => Price;
        set
        {
            Price = value;
            onPriceChange?.Invoke();
        }
    }

    public event Func<Slot> onBuyButtonClick;
    event Action onPriceChange;


    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.onWaveChange += v =>
        {
            if (v == 0) Price = 20;
        };

        _unitBuy = transform.Find("Button - UnitBuy").GetComponent<Button>();
        _price = transform.Find("Text (TMP) - Gold").GetComponent<TMP_Text>();

        _unitBuy.onClick.AddListener(() => UnitBuy(onBuyButtonClick?.Invoke()));
        onPriceChange += () => { _price.text = $"{Price}"; };
    }

    void OnDisable()
    {
        onPriceChange -= () => { _price.text = $"{Price}"; };
    }

    /// <summary>
    /// ������ �����ϴ� �Լ�
    /// </summary>
    void UnitBuy(Slot slot)
    {
        if (slot == null) return;

        Price += WEIGHT;
        //todo -> ���� ����ó��
    }
}
