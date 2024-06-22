using System.Collections.Generic;
using UnityEngine;

public class UnitLevelManager : MonoBehaviour
{
    const int EARLY_PRICE = 20;
    const int PRICE_WEIGHT = 2;
    const int POWER_WEIGHT = 10;

    Dictionary<UnitKind, int> _unitLevels = new Dictionary<UnitKind, int>();


    void Awake()
    {
        UnitLevelUpSlot[] slots = transform.GetComponentsInChildren<UnitLevelUpSlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            UnitKind unitKind = (UnitKind)i;
            _unitLevels.Add(unitKind, 0);
            UnitLevelUpSlot slot = slots[i];
            //slot.unitImg = ;
            slot.levelUp.onClick.AddListener(() =>
            {
                if (!LevelUpTry(unitKind)) return;

                int level = _unitLevels[unitKind];
                slot.level.text = $"{level}";
                slot.levelUpNeedGold.text = $"{EARLY_PRICE + (level * PRICE_WEIGHT)}";
            });
        }
    }

    void Start()
    {
        UIManager.Instance.Get<UnitBuyUI>().onUnitBuySuccess += (slot, unit) => BuyUnitApply(unit);
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
}