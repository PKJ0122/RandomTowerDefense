using System;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// ���� ���� UI / ����ó��
/// </summary>
public class UnitBuyUI : UIBase
{
    const int WEIGHT = 2;

    Button _unitBuy;
    TMP_Text _price;

    int _unitPrice;
    public int UnitPrice
    {
        get => _unitPrice;
        set
        {
            _unitPrice = value;
            OnPriceChange?.Invoke();
        }
    }

    public event Func<Slot> OnBuyButtonClick;
    event Action OnPriceChange;
    public event Action<UnitBase> OnUnitBuySuccess;


    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.OnWaveChange += v =>
        {
            if (v == 0) UnitPrice = 20;
        };

        _unitBuy = transform.Find("Button - UnitBuy").GetComponent<Button>();
        _price = transform.Find("Text (TMP) - Gold").GetComponent<TMP_Text>();

        _unitBuy.onClick.AddListener(() => UnitBuy(OnBuyButtonClick?.Invoke()));
        OnPriceChange += () => { _price.text = $"{UnitPrice}"; };
    }

    void OnDisable()
    {
        //GameManager.Instance.onWaveChange -= v =>
        //{
        //    if (v == 0) UnitPrice = 20;
        //};
        OnPriceChange -= () => { _price.text = $"{UnitPrice}"; };
    }

    /// <summary>
    /// ������ �����ϴ� �Լ�
    /// </summary>
    void UnitBuy(Slot slot)
    {
        if (slot == null || GameManager.Instance.Gold < UnitPrice) return;

        UnitBase randomUnit = RandomUnit(slot);
        OnUnitBuySuccess?.Invoke(randomUnit);
        GameManager.Instance.Gold -= UnitPrice;
        UnitPrice += WEIGHT;
    }

    /// <summary>
    /// ������ ������ ��ȯ���ִ� �Լ�
    /// </summary>
    public UnitBase RandomUnit(Slot slot)
    {
        UnitKind unitKind = (UnitKind)Random.Range(0, Enum.GetValues(typeof(UnitKind)).Length);
        UnitRank unitRank = RandomRank();

        UnitBase unit = ObjectPoolManager.Instance.Get(unitKind.ToString())
                                                  .Get()
                                                  .GetComponent<UnitBase>()
                                                  .UnitSet(slot,unitKind, unitRank);

        return unit;
    }

    /// <summary>
    /// ������ ����� ��ȯ���ִ� �Լ�
    /// </summary>
    UnitRank RandomRank()
    {
        int randomNumber = Random.Range(0, 100);

        if (randomNumber >= 98) return UnitRank.Legendary;
        else if (randomNumber >= 94) return UnitRank.Unique;
        else if (randomNumber >= 86) return UnitRank.Epic;
        else if (randomNumber >= 70) return UnitRank.Rare;
        else return UnitRank.Nomal;
    }
}
