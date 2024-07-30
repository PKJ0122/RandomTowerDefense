using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
            OnPriceChange?.Invoke();
        }
    }

    Dictionary<UnitRank, float> _unitRankPercentage = new Dictionary<UnitRank,float>();
    public Dictionary<UnitRank, float> UnitRankPercentage => _unitRankPercentage;

    public event Func<Slot> OnBuyButtonClick;
    event Action OnPriceChange;
    public event Action<UnitBase> OnUnitBuySuccess;


    protected override void Awake()
    {
        base.Awake();
        _unitBuy = transform.Find("Button - UnitBuy").GetComponent<Button>();
        _price = transform.Find("Text (TMP) - Gold").GetComponent<TMP_Text>();
        _unitBuy.onClick.AddListener(() => UnitBuy(OnBuyButtonClick?.Invoke()));
        _unitBuy.interactable = false;
        OnPriceChange += () => { _price.text = $"{UnitPrice}"; };

        GameManager.Instance.OnWaveChange += v =>
        {
            if (v == 0)
            {
                UnitPrice = 20;
                _unitBuy.interactable = true;
            };
        };

        GameManager.Instance.OnGameEnd += v => _unitBuy.interactable = false;

        UnitDatas unitDatas = UnitRepository.UnitDatas;
        UnitRankPercentage.Add(UnitRank.Legendary, unitDatas.legendaryPercentage);
        UnitRankPercentage.Add(UnitRank.Unique, unitDatas.uniquePercentage);
        UnitRankPercentage.Add(UnitRank.Epic, unitDatas.epicPercentage);
        UnitRankPercentage.Add(UnitRank.Rare, unitDatas.rarePercentage);
    }

    void OnDisable()
    {
        OnPriceChange -= () => { _price.text = $"{UnitPrice}"; };
    }

    /// <summary>
    /// 랜덤한 유닛을 반환해주는 함수
    /// </summary>
    public UnitBase RandomUnit(Slot slot)
    {
        UnitKind unitKind = (UnitKind)Random.Range(0, Enum.GetValues(typeof(UnitKind)).Length);
        UnitRank unitRank = RandomRank();

        UnitBase unit = ObjectPoolManager.Instance.Get(unitKind.ToString())
                                                  .Get()
                                                  .GetComponent<UnitBase>()
                                                  .UnitSet(slot, unitKind, unitRank);

        return unit;
    }

    /// <summary>
    /// 유닛을 구매하는 함수
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
    /// 랜덤한 등급을 반환해주는 함수
    /// </summary>
    UnitRank RandomRank()
    {
        float randomNumber = Random.Range(0f, 100f);

        float firstPercentage = 0;
        float latsPercentage = 0;

        for (int i = Enum.GetValues(typeof(UnitRank)).Length - 2; i > 0; i--)
        {
            latsPercentage += UnitRankPercentage[(UnitRank)i];

            if (firstPercentage <= randomNumber && randomNumber < latsPercentage)
                return (UnitRank)i;

            firstPercentage = latsPercentage;
        }

        return UnitRank.Nomal;
    }
}