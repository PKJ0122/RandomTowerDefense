using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemLevelUpUI : UIBase
{
    ItemDatas _itemDatas;

    Image _item;
    TMP_Text _level;
    Slider _itemCountS;
    TMP_Text _itemCountT;
    TMP_Text _levelUpNeedGold;
    TMP_Text _nowAbility;
    TMP_Text _nextAbility;
    Button _levelUp;
    Button _close;


    protected override void Awake()
    {
        base.Awake();
        _itemDatas = Resources.Load<ItemDatas>("ItemDatas");
        _item = transform.Find("Panel/Image/Image - Item/Image").GetComponent<Image>();
        _level = transform.Find("Panel/Image/Text (TMP) - ItemLevel").GetComponent<TMP_Text>();
        _itemCountS = transform.Find("Panel/Image/Slider - Count").GetComponent<Slider>();
        _itemCountT = _itemCountS.transform.Find("Text (TMP) - Count").GetComponent<TMP_Text>();
        _levelUpNeedGold = transform.Find("Panel/Image/Image - NeedGold/Text (TMP)").GetComponent<TMP_Text>();
        _nowAbility = transform.Find("Panel/Image/Image - Now/Text (TMP) - Now").GetComponent<TMP_Text>();
        _nextAbility = transform.Find("Panel/Image/Image - Next/Text (TMP) - Next").GetComponent<TMP_Text>();
        _levelUp = transform.Find("Panel/Image/GameObject/Button - LevelUp").GetComponent<Button>();
        _close = transform.Find("Panel/Image/GameObject/Button - Close").GetComponent<Button>();
        _close.onClick.AddListener(Hide);
    }

    public void Show(string itemName)
    {
        base.Show();
        Refresh(itemName);
    }

    public void Refresh(string itemName)
    {
        ItemLevelData itemLevelData = PlayerData.ItemLevels[itemName];
        ItemBase itemData = _itemDatas.Items[itemName];
        _item.sprite = itemData.itemImage;

        int itemLevel = Math.Min(9, itemLevelData.level);

        _level.text = $"{itemLevelData.level} Lv";
        int itemLevelUpNeedAmount = _itemDatas.itemLevelUpNeedAmount[itemLevel];
        _itemCountS.maxValue = itemLevelUpNeedAmount;
        _itemCountS.value = itemLevelData.Amount;
        _itemCountT.text = $"{itemLevelData.Amount} / {itemLevelUpNeedAmount}";
        int itemLevelUpNeedGold = _itemDatas.itemLevelUpNeedGold[itemLevel];
        _levelUpNeedGold.text = itemLevelUpNeedGold.ToString("N0");
        _nowAbility.text = itemData.Value.ToString();
        _nextAbility.text = (itemData.Value + itemData.weight).ToString();

        _levelUp.onClick.RemoveAllListeners();
        _levelUp.interactable = PlayerData.Instance.Gold >= itemLevelUpNeedGold &&
            itemLevelData.Amount >= itemLevelUpNeedAmount;
        _levelUp.onClick.AddListener(() =>
        {
            PlayerData.Instance.Gold -= itemLevelUpNeedGold;
            PlayerData.Instance.SetItemLevel(itemName, itemLevelUpNeedAmount);
            Refresh(itemName);
        });
    }
}
