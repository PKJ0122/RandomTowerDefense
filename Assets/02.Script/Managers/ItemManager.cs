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
        foreach (ItemBase item in _itemDatas.itemDatas)
        {
            item.TryUse();
        }
    }
}