using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodsShopPage : ShopPageBase
{
    const int AD_REWARD = 1000;
    const int PRICE = 500;
    const int GOLD_AMOUNT = 10000;

    EventHandler<EventArgs> OnAdClosedHandler;


    protected override void Awake()
    {
        base.Awake();
        _toggleObjectName = "Toggle - GoodsShop";
        PlayerData.OnLastShopChangeChange += ShopRefresh;

        Button tradeButton = Slots[1].GetComponent<Button>();
        tradeButton.onClick.AddListener(() =>
        {
            if (PlayerData.Instance.Diamond < PRICE) return;

            PlayerData.Instance.Diamond -= PRICE;
            PlayerData.Instance.Gold += GOLD_AMOUNT;
        });
        Button tenTradeButton = Slots[2].GetComponent<Button>();
        tenTradeButton.onClick.AddListener(() =>
        {
            if (PlayerData.Instance.Diamond < PRICE*10) return;

            PlayerData.Instance.Diamond -= PRICE*10;
            PlayerData.Instance.Gold += GOLD_AMOUNT*10;
        });
    }

    private void Start()
    {
        OnAdClosedHandler += (object sender, EventArgs args) =>
        {
            PlayerData.Instance.Diamond += AD_REWARD;
            PlayerData.Instance.PlayerDataContainer.adDiamondBuy = true;
            ShopRefresh();
            AdManager.Instance.RewardedAd.OnAdClosed -= OnAdClosedHandler;
        };
    }

    void OnDestroy()
    {
        PlayerData.OnLastShopChangeChange -= ShopRefresh;
    }

    protected override void ShopRefresh()
    {
        Slots[0].Find("Panel - Buy").gameObject.SetActive(PlayerData.Instance.PlayerDataContainer.adDiamondBuy);
        if (!PlayerData.Instance.PlayerDataContainer.adDiamondBuy)
        {
            Button adItem = Slots[0].GetComponent<Button>();
            adItem.onClick.RemoveAllListeners();
            adItem.onClick.AddListener(() =>
            {
                AdManager.Instance.RewardedAd.OnAdClosed += OnAdClosedHandler;
                adItem.onClick.RemoveAllListeners();
                AdManager.Instance.ShowAds();
            });
        }
    }
}
