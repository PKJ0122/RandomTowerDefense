using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 터치한 유닛의 정보를 보여주는 UI
/// </summary>
public class UnitInfoUI : UIBase
{
    Image _unitImage;
    TMP_Text _unitName;
    TMP_Text _unitRank;
    TMP_Text _unitPower;
    TMP_Text _unitDamage;
    Button _unitDamageReSet;

    Slot _currentSlot; //현재 선택된 유닛의 참조

    Button _unitSell;
    TMP_Text _unitSellPrice;

    Button _close;

    Action<float> _onDamageChangeHandler;

    protected override void Awake()
    {
        base.Awake();
        _unitImage = transform.Find("Panel/Image - Unit").GetComponent<Image>();
        _unitName = transform.Find("Panel/Panel - UnitName/Text (TMP) - UnitName").GetComponent<TMP_Text>();
        _unitRank = transform.Find("Panel/Panel - UnitRank/Text (TMP) - UnitRank").GetComponent<TMP_Text>();
        _unitPower = transform.Find("Panel/Panel - UnitPower/Text (TMP) - UnitPower").GetComponent<TMP_Text>();
        _unitDamage = transform.Find("Panel/Panel - UnitDamage/Text (TMP) - UnitDamage").GetComponent<TMP_Text>();
        _unitDamageReSet = transform.Find("Panel/Button - UnitDamageReSet").GetComponent<Button>();
        
        _unitSell = transform.Find("Panel/Image - UnitInfo/Button - UnitSell").GetComponent<Button>();
        _unitSellPrice = transform.Find("Panel/Image - UnitInfo/Button - UnitSell/Text (TMP) - SellPrice").GetComponent<TMP_Text>();

        _close = transform.Find("Panel/Button - CloseButton").GetComponent<Button>();

        _unitDamageReSet.onClick.AddListener(() => SlotManager.Slots[_currentSlot].DamageReSet());
        _close.onClick.AddListener(Hide);

        _onDamageChangeHandler += value => _unitDamage.text = $"{value}";

        onInputActionEnableChange += value =>
        {
            _unitDamageReSet.interactable = value;
            _unitSell.interactable = value;
            _close.interactable = value;
        };
    }

    public void Show(Slot slot)
    {
        base.Show();
        _currentSlot = slot;
        UnitBase unit = SlotManager.Slots[_currentSlot];
        UnitData unitData = UnitRepository.UnitKindDatas[unit.Kind];
        //_unitImage.sprite = ;
        _unitName.text = $"{unitData.unitName}";
        UnitRankData unitRankData = UnitRepository.UnitRankDatas[unit.Rank];
        _unitRank.text = $"{unitRankData.unitRankName}";
        _unitRank.color = unitRankData.unitRankColor;
        _unitPower.text = $"{unit.Power}";
        _unitSellPrice.text = UnitSellPrice().ToString();
        
        _onDamageChangeHandler?.Invoke(unit.Damage);
        unit.onDamageChange += _onDamageChangeHandler;
    }

    public override void Hide()
    {
        base.Hide();
        UnitBase unit = SlotManager.Slots[_currentSlot];
        unit.onDamageChange -= _onDamageChangeHandler;
        _currentSlot = null;
    }

    int UnitSellPrice()
    {
        UnitBase unit = SlotManager.Slots[_currentSlot];
        return (int)(Mathf.Pow(3, (int)unit.Rank) * UIManager.Instance.Get<UnitBuyUI>().UnitPrice / 2);
    }
}