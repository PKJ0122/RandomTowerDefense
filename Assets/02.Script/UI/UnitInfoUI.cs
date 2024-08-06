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

    Transform _beyondEnforce;
    TMP_Text _enforce;
    TMP_Text _additionalATK;
    Image _material;
    TMP_Text _materialRank;
    Transform _maxEnforce;
    Button _beyondEnforceUp;

    Slot _currentSlot; //현재 선택된 유닛의 참조

    Action<float> _onPowerChangeHandler;
    Action<float> _onDamageChangeHandler;
    Action<int, int> _onMpChangeHandler;

    public event Action<UnitBase> OnUnitMix;
    public event Action<int> OnUnitSell;

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

        _beyondEnforce = transform.Find("Panel/Image - BeyondEnforce").GetComponent<Transform>();
        _enforce = _beyondEnforce.transform.Find("Panel - Enforce/Text (TMP)").GetComponent<TMP_Text>();
        _additionalATK = _beyondEnforce.transform.Find("Panel - AdditionalATK/Text (TMP)").GetComponent<TMP_Text>();
        _material = _beyondEnforce.transform.Find("Image - Material/Image - Unit").GetComponent<Image>();
        _materialRank = _beyondEnforce.transform.Find("Image - Material/Text (TMP) - Rank").GetComponent<TMP_Text>();
        _maxEnforce = _beyondEnforce.transform.Find("Panel - MaxEnforce").GetComponent<Transform>();
        _beyondEnforceUp = _beyondEnforce.transform.Find("Button - EnforceUp").GetComponent<Button>();
        

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
        OnInputActionEnableChange += value =>
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
        _beyondEnforceUp.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.Get<BeyondEnforceUI>().Show((BeyondBase)SlotManager.Slots[_currentSlot]);
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

        unit.OnPowerChange += _onPowerChangeHandler;
        unit.OnDamageChange += _onDamageChangeHandler;
        _onDamageChangeHandler?.Invoke(unit.Damage);
        unit.OnMpChange += _onMpChangeHandler;
        _onMpChangeHandler?.Invoke(unit.Mp, skill.needMp);

        _beyondEnforce.gameObject.SetActive(unit.Rank == UnitRank.Beyond);
        if (unit.Rank == UnitRank.Beyond)
        {
            BeyondBase beyond = (BeyondBase)unit;
            bool maxEnforce = beyond.EnforceMaterial == null;
            _maxEnforce.gameObject.SetActive(maxEnforce);

            if (maxEnforce) return;

            _enforce.text = $"+ {beyond.Enforce}";
            _additionalATK.text = $"+ {beyond.AddAdditionalATK()}";

            UnitInfo material = beyond.EnforceMaterial.Value;

            _material.sprite = UnitRepository.UnitKindDatas[material.unitKind].unitImg;

            UnitRankData rankData = UnitRepository.UnitRankDatas[material.unitRank];
            _materialRank.text = rankData.unitRankName;
            _materialRank.color = rankData.unitRankColor;
        }
    }

    public override void Hide()
    {
        base.Hide();
        UnitBase unit = SlotManager.Slots[_currentSlot];
        unit.OnPowerChange -= _onPowerChangeHandler;
        unit.OnDamageChange -= _onDamageChangeHandler;
        unit.OnMpChange -= _onMpChangeHandler;
    }

    void UnitSell()
    {
        Hide();
        GameManager.Instance.Gold += UnitSellPrice();
        OnUnitSell?.Invoke(UnitSellPrice());
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
        UnitBase mixUnit = UnitFactory.Instance.UnitCreat<UnitBase>(_currentSlot, mixUnitRank);
        OnUnitMix?.Invoke(mixUnit);
    }
}