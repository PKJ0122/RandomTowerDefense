using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DailyShopPage : ShopPageBase
{
    ItemDatas _itemDatas;

    Action OnLastShopChangeHandler;



    protected override void Awake()
    {
        base.Awake();
        _toggleObjectName = "Toggle - DailyShop";
        _itemDatas = Resources.Load<ItemDatas>("ItemDatas");
        OnLastShopChangeHandler += () =>
        {
            RandomDailyShop();
            ShopRefresh();
        };
        PlayerData.Instance.OnLastShopChangeChange += OnLastShopChangeHandler;
    }

    public void RandomDailyShop()
    {
        for (int i = 0; i < 5; i++)
        {
            int randomItem = Random.Range(0, _itemDatas.Items.Count);
            string itemName = _itemDatas.itemDatas[randomItem].itemName;
            int randomKind = Random.Range(GOLD, DIAMOND + 1);
            int randomPrice = randomKind == GOLD ? 9999 : 700;

            ShopSaveData newData = new ShopSaveData(itemName, randomPrice, randomKind);

            PlayerData.Instance.SetShopSaveData(i, newData);
        }
    }

    protected override void ShopRefresh()
    {
        Transform buy = Slots[0].Find("Panel - Buy").GetComponent<Transform>();
        buy.gameObject.SetActive(PlayerData.PlayerDataContainer.freeDiamondBuy);

        if (!PlayerData.PlayerDataContainer.freeDiamondBuy)
        {
            Button freeDiamond = Slots[0].GetComponent<Button>();
            freeDiamond.onClick.RemoveAllListeners();
            freeDiamond.onClick.AddListener(() =>
            {
                PlayerData.PlayerDataContainer.freeDiamondBuy = true;
                buy.gameObject.SetActive(true);
                PlayerData.Instance.Diamond += 500;
                freeDiamond.onClick.RemoveAllListeners();
            });
        }

        List<ShopSaveData> shopDatas = PlayerData.PlayerDataContainer.shopSaveDatas;

        for (int i = 1; i < Slots.Length; i++)
        {
            if (shopDatas == null || shopDatas.Count < i) return;

            ShopSaveData shopData = shopDatas[i - 1];

            Button itemBuyButton = Slots[i].GetComponent<Button>();
            Image itemImage = Slots[i].Find("Image - Item").GetComponent<Image>();
            TMP_Text itemPrice = Slots[i].Find("Text (TMP) - Price").GetComponent<TMP_Text>();
            Transform buyObject = Slots[i].Find("Panel - Buy").GetComponent<Transform>();
            itemImage.sprite = _itemDatas.Items[shopData.itemName].itemImage;
            itemPrice.text = $"<size=80><sprite={shopData.priceKind}></size>{shopData.price}";
            buyObject.gameObject.SetActive(shopData.isBuy);

            if (shopData.isBuy) continue;

            itemBuyButton.onClick.RemoveAllListeners();

            Func<bool> action = () =>
            {
                if (shopData.priceKind == GOLD)
                {
                    if (PlayerData.Instance.Gold < shopData.price) return false;

                    PlayerData.Instance.Gold -= shopData.price;
                }
                else if (shopData.priceKind == DIAMOND)
                {
                    if (PlayerData.Instance.Diamond < shopData.price) return false;

                    PlayerData.Instance.Diamond -= shopData.price;
                }

                shopData.isBuy = true;
                buyObject.gameObject.SetActive(true);
                PlayerData.Instance.SetItemAmount(shopData.itemName, 1);
                itemBuyButton.onClick.RemoveAllListeners();
                return true;
            };
            itemBuyButton.onClick.AddListener(() =>
            {
                UIManager.Instance.Get<ItemBuyInfoUI>().Show(action,shopData);
            });
        }
    }
}