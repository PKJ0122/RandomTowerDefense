using System;
using System.Collections.Generic;
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

    Image _unitSkillImage;
    TMP_Text _unitSkillName;
    TMP_Text _unitSkillDisciption;
    Slider _unitSkillMpS;
    TMP_Text _unitSkillMpT;

    Button _unitSell;
    TMP_Text _unitSellPrice;
    Button _unitMove;
    Button _unitMix;

    Button _close;

    Slot _currentSlot; //현재 선택된 유닛의 참조

    Action<float> _onPowerChangeHandler;
    Action<float> _onDamageChangeHandler;
    Action<int, int> _onMpChangeHandler;

    public event Action<UnitBase> onUnitMix;

    protected override void Awake()
    {
        base.Awake();
        _unitImage = transform.Find("Panel/Image - Unit").GetComponent<Image>();
        _unitName = transform.Find("Panel/Panel - UnitName/Text (TMP) - UnitName").GetComponent<TMP_Text>();
        _unitRank = transform.Find("Panel/Panel - UnitRank/Text (TMP) - UnitRank").GetComponent<TMP_Text>();
        _unitPower = transform.Find("Panel/Panel - UnitPower/Text (TMP) - UnitPower").GetComponent<TMP_Text>();
        _unitDamage = transform.Find("Panel/Panel - UnitDamage/Text (TMP) - UnitDamage").GetComponent<TMP_Text>();
        _unitDamageReSet = transform.Find("Panel/Button - UnitDamageReSet").GetComponent<Button>();

        _unitSkillImage = transform.Find("Panel/Image - UnitSkill/Image - Skill").GetComponent<Image>();
        _unitSkillName = transform.Find("Panel/Image - UnitSkill/Text (TMP) - SkillName").GetComponent<TMP_Text>();
        _unitSkillDisciption = transform.Find("Panel/Image - UnitSkill/Text (TMP) - SkillDiscription").GetComponent<TMP_Text>();
        _unitSkillMpS = transform.Find("Panel/Image - UnitSkill/Slider - Mp").GetComponent<Slider>();
        _unitSkillMpT = transform.Find("Panel/Image - UnitSkill/Slider - Mp/Text (TMP) - Mp").GetComponent<TMP_Text>();

        _unitSell = transform.Find("Panel/Image - UnitInfo/Button - UnitSell").GetComponent<Button>();
        _unitSellPrice = transform.Find("Panel/Image - UnitInfo/Button - UnitSell/Text (TMP) - SellPrice").GetComponent<TMP_Text>();
        _unitSell.onClick.AddListener(UnitSell);
        _unitMove = transform.Find("Panel/Image - UnitInfo/Button - UnitMove").GetComponent<Button>();
        _unitMix = transform.Find("Panel/Image - UnitInfo/Button - UnitMix").GetComponent<Button>();
        _unitMix.onClick.AddListener(UnitMix);

        _close = transform.Find("Panel/Button - CloseButton").GetComponent<Button>();

        _unitDamageReSet.onClick.AddListener(() => SlotManager.Slots[_currentSlot].DamageReSet());
        _close.onClick.AddListener(Hide);

        _onPowerChangeHandler += value =>
        {
            _unitPower.text = $"{value}";
        };
        _onDamageChangeHandler += value =>
        {
            _unitDamage.text = $"{value}";
        };
        _onMpChangeHandler += (mp, skillNeedMp) =>
        {
            _unitSkillMpS.value = mp;
            _unitSkillMpT.text = $"{mp} / {skillNeedMp}";
        };
        onInputActionEnableChange += value =>
        {
            _unitDamageReSet.interactable = value;
            _unitSell.interactable = value;
            _unitMove.interactable = value;
            _unitMix.interactable = value;
            _close.interactable = value;
        };
    }

    private void Start()
    {
        _unitMove.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.Get<UnitMoveUI>().Show(_currentSlot);
        });
    }

    public void Show(Slot slot)
    {
        base.Show();
        _currentSlot = slot;
        UnitBase unit = SlotManager.Slots[_currentSlot];
        UnitData unitData = UnitRepository.UnitKindDatas[unit.Kind];
        _unitImage.sprite = unitData.unitImg;
        _unitName.text = $"{unitData.unitName}";
        UnitRankData unitRankData = UnitRepository.UnitRankDatas[unit.Rank];
        _unitRank.text = $"{unitRankData.unitRankName}";
        _unitRank.color = unitRankData.unitRankColor;
        _unitPower.text = $"{unit.Power}";
        _unitSellPrice.text = UnitSellPrice().ToString();

        SkillBase skill = unitData.skill;
        _unitSkillImage.sprite = skill.skillImg;
        _unitSkillName.text = $"{skill.skillname}";
        _unitSkillDisciption.text = $"{skill.description}";
        _unitSkillMpS.maxValue = skill.needMp;

        unit.onPowerChange += _onPowerChangeHandler;
        unit.onDamageChange += _onDamageChangeHandler;
        _onDamageChangeHandler?.Invoke(unit.Damage);
        unit.onMpChange += _onMpChangeHandler;
        _onMpChangeHandler?.Invoke(unit.Mp, skill.needMp);
    }

    public override void Hide()
    {
        base.Hide();
        UnitBase unit = SlotManager.Slots[_currentSlot];
        unit.onPowerChange -= _onPowerChangeHandler;
        unit.onDamageChange -= _onDamageChangeHandler;
        unit.onMpChange -= _onMpChangeHandler;
    }

    void UnitSell()
    {
        Hide();
        GameManager.Instance.Gold += UnitSellPrice();
        SlotManager.Slots[_currentSlot].RelasePool();
    }

    int UnitSellPrice()
    {
        UnitBase unit = SlotManager.Slots[_currentSlot];
        return (int)(Mathf.Pow(3, (int)unit.Rank) * UIManager.Instance.Get<UnitBuyUI>().UnitPrice / 2);
    }

    void UnitMix()
    {
        UnitBase baseUnit = SlotManager.Slots[_currentSlot];
        UnitKind unitKind = baseUnit.Kind;
        UnitRank unitRank = baseUnit.Rank;

        if (unitRank == UnitRank.Legendary) return;

        List<UnitBase> mixUnits = new List<UnitBase>(3) { baseUnit };


        List<UnitBase> units = GameManager.Instance.Units[unitKind];

        if (units.Count < 3) return;

        foreach (UnitBase unit in units)
        {
            if (unit.Rank == unitRank && unit != baseUnit)
            {
                mixUnits.Add(unit);
                if (mixUnits.Count >= 3) break;
            }
        }

        if (mixUnits.Count < 3) return;

        Hide();

        foreach (UnitBase unit in mixUnits)
        {
            unit.RelasePool();
        }

        UnitRank mixUnitRank = (UnitRank)((int)unitRank + 1);
        UnitBase mixUnit = UIManager.Instance.Get<UnitBuyUI>().RandomUnit(_currentSlot);
        mixUnit.UnitSet(_currentSlot, mixUnit.Kind, mixUnitRank);
        onUnitMix?.Invoke(mixUnit);
    }
}