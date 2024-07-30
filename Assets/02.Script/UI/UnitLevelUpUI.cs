using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class UnitLevelUpUI : UIBase
{
    const int EARLY_PRICE = 100;
    const int PRICE_WEIGHT = 100;
    const float POWER_WEIGHT = 0.1f;

    Dictionary<UnitKind, int> _unitLevels = new Dictionary<UnitKind, int>();

    Button _close;

    public event Action OnUnitLevelUpSuccess;


    protected override void Awake()
    {
        base.Awake();
        _close = transform.Find("Button - CloseButton").GetComponent<Button>();
        _close.onClick.AddListener(Hide);
    }

    void Start()
    {
        UnitLevelUpSlot[] slots = transform.GetComponentsInChildren<UnitLevelUpSlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            UnitKind unitKind = (UnitKind)i;
            _unitLevels.Add(unitKind, 0);
            UnitLevelUpSlot slot = slots[i];
            slot.unitImg.sprite = UnitRepository.UnitKindDatas[unitKind].unitImg;
            slot.levelUp.onClick.AddListener(() =>
            {
                if (!LevelUpTry(unitKind)) return;

                int level = _unitLevels[unitKind];
                slot.level.text = $"{level} Lv";
                slot.levelUpNeedGold.text = $"{EARLY_PRICE + (level * PRICE_WEIGHT)}";
                PurchasedUnitApply(unitKind);
            });
        }
        UIManager.Instance.Get<UnitBuyUI>().OnUnitBuySuccess += (unit) => BuyUnitApply(unit);
        UIManager.Instance.Get<GameEndUI>().OnReStartButtonClick += () =>
        {
            for (int i = 0; i < slots.Length; i++)
            {
                UnitLevelUpSlot slot = slots[i];
                slot.level.text = $"0 Lv";
                slot.levelUpNeedGold.text = $"100";
                _unitLevels[(UnitKind)i] = 0;
            }
        };
    }

    /// <summary>
    /// 레벨업을 시도 하는 함수
    /// </summary>
    bool LevelUpTry(UnitKind unitKind)
    {
        int level = _unitLevels[unitKind];
        int levelUpNeedGold = EARLY_PRICE + (level * PRICE_WEIGHT);

        if (GameManager.Instance.Gold < levelUpNeedGold) return false;

        GameManager.Instance.Gold -= levelUpNeedGold;
        _unitLevels[unitKind]++;
        OnUnitLevelUpSuccess.Invoke();
        return true;
    }

    /// <summary>
    /// 구매한 유닛 이미찍은 레벨업 적용
    /// </summary>
    /// <param name="unit"></param>
    void BuyUnitApply(UnitBase unit)
    {
        int unitLevel = _unitLevels[unit.Kind];
        unit.Power += unit.Power * (unitLevel * POWER_WEIGHT);
    }

    /// <summary>
    /// 이미 구매한 유닛 레벨업 적용
    /// </summary>
    void PurchasedUnitApply(UnitKind unitKind)
    {
        if (!GameManager.Instance.Units.TryGetValue(unitKind, out List<UnitBase> units)) return;

        float[] unitPowerDatas = UnitRepository.UnitKindDatas[unitKind].unitPowerDatas;
        float powerWeight = _unitLevels[unitKind] * POWER_WEIGHT;

        foreach (UnitBase unit in units)
        {
            float unitBasePower = unitPowerDatas[(int)unit.Rank];
            unit.Power = unitBasePower + (unitBasePower * powerWeight);
        }
    }
}