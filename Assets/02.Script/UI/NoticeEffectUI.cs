using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeEffectUI : UIBase
{
    ItemDatas _itemDatas;
    ItemEffect _itemEffectPrefab;
    Transform _itemEffectLocation;

    protected override void Awake()
    {
        base.Awake();
        _itemDatas = Resources.Load<ItemDatas>("ItemDatas");
        _itemEffectPrefab = _itemDatas.itemEffect;
        _itemEffectLocation = transform.Find("Panel/Scroll View/Viewport/Content").GetComponent<Transform>();
        ObjectPoolManager.Instance.CreatePool("itemEffectPrefab", _itemEffectPrefab, 8);
    }

    /// <summary>
    /// ������ ����Ȳ,�̼� Ŭ���� ��Ȳ�� �˷��ִ� UI
    /// </summary>
    /// <param name="itemImage">�̹����� ���� ��� null</param>
    public void Show(Sprite itemImage, string detail)
    {
        ItemEffect obj = ObjectPoolManager.Instance.Get("itemEffectPrefab")
                                                   .Get()
                                                   .GetComponent<ItemEffect>()
                                                   .Denote(itemImage, detail);
        obj.transform.SetParent(_itemEffectLocation, false);
        obj.transform.SetAsLastSibling();
    }
}