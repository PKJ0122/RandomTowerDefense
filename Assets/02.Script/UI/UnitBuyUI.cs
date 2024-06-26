using System;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// 유닛 구매 UI / 구매처리
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
            onPriceChange?.Invoke();
        }
    }

    public event Func<Slot> onBuyButtonClick;
    event Action onPriceChange;
    public event Action<Slot,UnitBase> onUnitBuySuccess;


    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.onWaveChange += v =>
        {
            if (v == 0) UnitPrice = 20;
        };

        _unitBuy = transform.Find("Button - UnitBuy").GetComponent<Button>();
        _price = transform.Find("Text (TMP) - Gold").GetComponent<TMP_Text>();

        _unitBuy.onClick.AddListener(() => UnitBuy(onBuyButtonClick?.Invoke()));
        onPriceChange += () => { _price.text = $"{UnitPrice}"; };
    }

    void OnDisable()
    {
        //GameManager.Instance.onWaveChange -= v =>
        //{
        //    if (v == 0) UnitPrice = 20;
        //};
        onPriceChange -= () => { _price.text = $"{UnitPrice}"; };
    }

    /// <summary>
    /// 유닛을 구매하는 함수
    /// </summary>
    void UnitBuy(Slot slot)
    {
        if (slot == null || GameManager.Instance.Gold < UnitPrice) return;

        UnitBase randomUnit = RandomUnit(slot);
        onUnitBuySuccess?.Invoke(slot,randomUnit);
        GameManager.Instance.Gold -= UnitPrice;
        UnitPrice += WEIGHT;
    }

    /// <summary>
    /// 랜덤한 유닛을 반환해주는 함수
    /// </summary>
    UnitBase RandomUnit(Slot slot)
    {
        UnitKind unitKind = (UnitKind)Random.Range(0, Enum.GetValues(typeof(UnitKind)).Length);
        UnitRank unitRank = RandomRank();

        UnitBase unit = ObjectPoolManager.Instance.Get(unitKind.ToString())
                                                  .Get()
                                                  .GetComponent<UnitBase>()
                                                  .UnitSet(slot,unitKind, unitRank);

        return unit;
    }

    public UnitBase RandomUnit(Slot slot, UnitRank unitRank)
    {
        UnitKind unitKind = (UnitKind)Random.Range(0, Enum.GetValues(typeof(UnitKind)).Length);
        UnitRank spawnUnitRank = (UnitRank)((int)unitRank + 1);

        UnitBase unit = ObjectPoolManager.Instance.Get(unitKind.ToString())
                                                  .Get()
                                                  .GetComponent<UnitBase>()
                                                  .UnitSet(slot, unitKind, spawnUnitRank);

        return unit;
    }

    /// <summary>
    /// 랜덤한 등급을 반환해주는 함수
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
