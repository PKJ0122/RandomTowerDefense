using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : ItemBase
{
    protected override void Use()
    {
        UIManager.Instance.Get<MissionUI>().ClearGold += (int)Value;
        UIManager.Instance.Get<MissionUI>().OnMissionClear += () =>
        {
            UIManager.Instance.Get<ItemUseEffectUI>().Show(itemImage,itemName);
        };
    }
}
