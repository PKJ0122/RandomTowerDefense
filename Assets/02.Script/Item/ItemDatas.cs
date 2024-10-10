using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatas", menuName = "ScriptableObject/ItemDatas")]
[Serializable]
public class ItemDatas : ScriptableObject
{
    Dictionary<string, ItemBase> _items;
    public Dictionary<string, ItemBase> Items
    {
        get
        {
            if (_items == null)
            {
                _items = new Dictionary<string, ItemBase>();
                foreach (ItemBase item in itemDatas)
                {
                    _items.Add(item.itemName, item);
                }
            }
            return _items;
        }
    }

    public ItemBase[] itemDatas;

    public int[] itemLevelUpNeedAmount;

    public int[] itemLevelUpNeedGold;

    public ItemEffect itemEffect;
}
