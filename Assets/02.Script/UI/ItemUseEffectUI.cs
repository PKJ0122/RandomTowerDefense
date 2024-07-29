using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseEffectUI : UIBase
{
    ItemDatas _itemDatas;
    ItemEffect _itemEffectPrefab;
    Transform _itemEffectLocation;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            int Randomitem = Random.Range(0, _itemDatas.itemDatas.Length);

            ItemBase item = _itemDatas.itemDatas[Randomitem];

            Show(item.itemImage, item.itemName);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _itemDatas = Resources.Load<ItemDatas>("ItemDatas");
        _itemEffectPrefab = _itemDatas.itemEffect;
        _itemEffectLocation = transform.Find("Panel/Scroll View/Viewport/Content").GetComponent<Transform>();
        ObjectPoolManager.Instance.CreatePool("itemEffectPrefab", _itemEffectPrefab, 5);
    }

    public void Show(Sprite itemImage, string itemName)
    {
        ItemEffect obj = ObjectPoolManager.Instance.Get("itemEffectPrefab")
                                                   .Get()
                                                   .GetComponent<ItemEffect>()
                                                   .Denote(itemImage, itemName);
        obj.transform.SetParent(_itemEffectLocation, false);
        obj.transform.SetAsLastSibling();
    }
}