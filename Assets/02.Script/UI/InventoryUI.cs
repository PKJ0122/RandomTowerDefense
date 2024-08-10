using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : UIBase
{
    Dictionary<string, Button> _slots = new Dictionary<string, Button>();

    ItemDatas _itemDatas;
    Button _slotPrefab;
    Transform _slotLocation;

    Button _close;

    Action<ItemLevelData> OnItemDataChangeHandler;


    protected override void Awake()
    {
        base.Awake();
        _itemDatas = Resources.Load<ItemDatas>("ItemDatas");
        _slotPrefab = Resources.Load<Button>("Button - InventoryItemSlot");
        _slotLocation = transform.Find("Panel/Image/Scroll View/Viewport/Content").GetComponent<Transform>();
        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _close.onClick.AddListener(Hide);
    }

    private void Start()
    {
        foreach (KeyValuePair<string, ItemBase> item in _itemDatas.Items)
        {
            Button slot = Instantiate(_slotPrefab, _slotLocation);
            slot.onClick.AddListener(() => UIManager.Instance.Get<ItemInfoUI>().Show(item.Key));
            Image slotItemImg = slot.transform.Find("Image - Item").GetComponent<Image>();
            slotItemImg.sprite = _itemDatas.Items[item.Key].itemImage;
            SlotRefresh(slot, item.Key);
            _slots.Add(item.Key, slot);
        }

        OnItemDataChangeHandler += itemLevelData =>
        {
            SlotRefresh(_slots[itemLevelData.itemName], itemLevelData.itemName);
        };

        PlayerData.Instance.OnItemDataChange += OnItemDataChangeHandler;
    }

    void SlotRefresh(Button slot, string itemName)
    {
        Image slotItemImg = slot.transform.Find("Image - Item").GetComponent<Image>();
        Slider itemCountS = slot.transform.Find("Slider - Count").GetComponent<Slider>();
        TMP_Text itemCountT = itemCountS.transform.Find("Text (TMP) - Count").GetComponent<TMP_Text>();

        bool isItemPossess = PlayerData.Instance.IsItemPossess(itemName);
        slotItemImg.color = isItemPossess ? Color.white : Color.black;

        if (!isItemPossess)
        {
            itemCountS.value = 0;
            itemCountT.text = "0 / 1";
            return;
        }
        ItemLevelData itemLevelData = PlayerData.ItemLevels[itemName];

        int level = Math.Min(9, itemLevelData.level);

        int itemLevelUpNeedAmount = _itemDatas.itemLevelUpNeedAmount[level];

        itemCountS.maxValue = itemLevelUpNeedAmount;
        itemCountS.value = itemLevelData.Amount;
        itemCountT.text = $"{itemLevelData.Amount} / {itemLevelUpNeedAmount}";
    }
}
