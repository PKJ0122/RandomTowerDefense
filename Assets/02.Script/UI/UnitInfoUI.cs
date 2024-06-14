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

    UnitBase _currentUnit; //현재 선택된 유닛의 참조


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

        _unitDamageReSet.onClick.AddListener(() => _currentUnit.DamageReSet());
        _close.onClick.AddListener(Hide);
    }

    public void Show(UnitBase unit)
    {
        base.Show();
        _currentUnit = unit;
        UnitData unitData = UnitRepository.UnitKindDatas[_currentUnit.Kind];
        //_unitImage.sprite = ;
        _unitName.text = $"{unitData.unitName}";
        UnitRankData unitRankData = UnitRepository.UnitRankDatas[_currentUnit.Rank];
        _unitRank.text = $"{unitRankData.unitRankName}";
        _unitRank.color = unitRankData.unitRankColor;
        _unitPower.text = $"{_currentUnit.Power}";
        DamageSet(_currentUnit.Damage);

        _currentUnit.onDamageChange += v => DamageSet(v);
    }

    public override void Hide()
    {
        base.Hide();
        _currentUnit.onDamageChange -= v => DamageSet(v);
        _currentUnit = null;
    }

    void DamageSet(float damage)
    {
        _unitDamage.text = $"{damage}";
    }
}