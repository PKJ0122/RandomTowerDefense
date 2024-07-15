using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUI : UIBase
{
    ItemDatas _itemDatas;
    TMP_Text _name;
    TMP_Text _discription;
    Image _itemImg;
    Button _close;


    protected override void Awake()
    {
        base.Awake();
        _itemDatas = Resources.Load<ItemDatas>("ItemDatas");
        _name = transform.Find("Panel/Image/Image - Name/Text (TMP)").GetComponent<TMP_Text>();
        _discription = transform.Find("Panel/Image/Image - Discription/Text (TMP)").GetComponent<TMP_Text>();
        _itemImg = transform.Find("Panel/Image/Image - Item/Image").GetComponent<Image>();
        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _close.onClick.AddListener(Hide);
    }

    public void Show(string itemName)
    {
        base.Show();
        Debug.Log(itemName);
        SortingOrder = 100;
        ItemBase itemBase = _itemDatas.Items[itemName];
        _name.text = itemBase.itemName;
        _discription.text = itemBase.description;
        _itemImg.sprite = itemBase.itemImage;
    }
}
