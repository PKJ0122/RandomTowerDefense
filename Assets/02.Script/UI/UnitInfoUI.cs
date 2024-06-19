using System;
using TMPro;
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
    Button _close;

    Slot _currentSlot; //현재 선택된 유닛의 참조


    protected override void Awake()
    {
        base.Awake();
        _unitImage = transform.Find("Panel/Image - Unit").GetComponent<Image>();
        _unitName = transform.Find("Panel/Panel - UnitName/Text (TMP) - UnitName").GetComponent<TMP_Text>();
        _unitRank = transform.Find("Panel/Panel - UnitRank/Text (TMP) - UnitRank").GetComponent<TMP_Text>();
        _unitPower = transform.Find("Panel/Panel - UnitPower/Text (TMP) - UnitPower").GetComponent<TMP_Text>();
        _unitDamage = transform.Find("Panel/Panel - UnitDamage/Text (TMP) - UnitDamage").GetComponent<TMP_Text>();
        _unitDamageReSet = transform.Find("Panel/Button - UnitDamageReSet").GetComponent<Button>();
        _close = transform.Find("Panel/Button - CloseButton").GetComponent<Button>();

        _unitDamageReSet.onClick.AddListener(() => SlotManager.Slots[_currentSlot].DamageReSet());
        _close.onClick.AddListener(Hide);
    }

    public void Show(Slot slot)
    {
        base.Show();
        UnitBase unit = SlotManager.Slots[slot];
        UnitData unitData = UnitRepository.UnitKindDatas[unit.Kind];
        //_unitImage.sprite = ;
        _unitName.text = $"{unitData.unitName}";
        UnitRankData unitRankData = UnitRepository.UnitRankDatas[unit.Rank];
        _unitRank.text = $"{unitRankData.unitRankName}";
        _unitRank.color = unitRankData.unitRankColor;
        _unitPower.text = $"{unit.Power}";
        DamageSet(unit.Damage);

        unit.onDamageChange += v => DamageSet(v);
    }

    public override void Hide()
    {
        base.Hide();
        UnitBase unit = SlotManager.Slots[_currentSlot];
        unit.onDamageChange -= v => DamageSet(v);
        unit = null;
    }

    void DamageSet(float damage)
    {
        _unitDamage.text = $"{damage}";
    }
}