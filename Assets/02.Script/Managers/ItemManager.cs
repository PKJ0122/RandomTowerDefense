using UnityEngine;

public class ItemManager : MonoBehaviour
{
    ItemDatas _itemDatas;


    private void Awake()
    {
        _itemDatas = Resources.Load<ItemDatas>("ItemDatas");
    }

    void Start()
    {
        foreach (var item in _itemDatas.itemDatas)
        {
            PlayerData.Instance.SetItemAmount($"{item.itemName}", 1);
        }
        foreach (ItemBase item in _itemDatas.itemDatas)
        {
            item.TryUse();
        }
    }
}