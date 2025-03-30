using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemBuyInfoUI : UIBase
{
    Image _item;
    TMP_Text _itemName;
    TMP_Text _discription;
    Button _buy;
    Button _close;
    TMP_Text _price;

    ItemDatas _itemDatas;


    protected override void Awake()
    {
        base.Awake();
        _itemDatas = Resources.Load<ItemDatas>("ItemDatas");

        _item = transform.Find("Panel/Image/Image - Item/Image").GetComponent<Image>();
        _itemName = transform.Find("Panel/Image/Image - Name/Text (TMP)").GetComponent<TMP_Text>();
        _discription = transform.Find("Panel/Image/Image - Discription/Text (TMP)").GetComponent<TMP_Text>();
        _buy = transform.Find("Panel/Image/GameObject/Button - Buy").GetComponent<Button>();
        _buy.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _close = transform.Find("Panel/Image/GameObject/Button - Close").GetComponent<Button>();
        _price = _buy.transform.Find("Text (TMP)").GetComponent<TMP_Text>();

        _close.onClick.AddListener(Hide);
        _close.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        PlayerData.Instance.OnLastShopChangeChange += () =>
        {
            _buy.onClick.RemoveAllListeners();
            _buy.onClick.AddListener(() => UIManager.Instance.Get<PopUpUI>().Show("구매가 불가능한 상태입니다."));
        };
    }

    public void Show(Func<bool> action,ShopSaveData shopSaveData)
    {
        base.Show();
        Action onBuyButtonClick = () =>
        {
            bool buySucsse = action.Invoke();
            if (buySucsse)
            {
                Hide();
                UIManager.Instance.Get<ItemInfoUI>().Show(shopSaveData.itemName);
            }
            else
            {
                SoundManager.Instance.PlaySound(SFX.Fail);
            }
        };
        _buy.onClick.RemoveAllListeners();
        _buy.onClick.AddListener(() => onBuyButtonClick.Invoke());

        ItemBase itemData = _itemDatas.Items[shopSaveData.itemName];
        _item.sprite = itemData.itemImage;
        _itemName.text = itemData.ItemName();
        _discription.text = itemData.Description();
        _price.text = $"<sprite={shopSaveData.priceKind}> {shopSaveData.price}";
    }
}
