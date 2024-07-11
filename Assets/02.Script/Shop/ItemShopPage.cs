using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopPage : ShopPageBase
{
    TMP_Text _itemSummons;

    Action<int> OnItemSummonsChangeHandler;
    EventHandler<EventArgs> OnAdClosedHandler;


    protected override void Awake()
    {
        base.Awake();
        _toggleObjectName = "Toggle - ItemShop";
        OnItemSummonsChangeHandler += value =>
        {
            _itemSummons.text = value.ToString("N0");
        };
        OnAdClosedHandler += (object sender, EventArgs args) =>
        {

            AdManager.Instance.RewardedAd.OnAdClosed -= OnAdClosedHandler;
        };
        PlayerData.OnItemSummonsChange += OnItemSummonsChangeHandler;
        PlayerData.OnLastShopChangeChange += ShopRefresh;
    }

    private void OnDestroy()
    {
        PlayerData.OnItemSummonsChange -= OnItemSummonsChangeHandler;
        PlayerData.OnLastShopChangeChange -= ShopRefresh;
    }

    protected override void ShopRefresh()
    {
        if (!PlayerData.Instance.PlayerDataContainer.adItemBuy)
        {
            Button adItem = Slots[0].GetComponent<Button>();
            adItem.onClick.RemoveAllListeners();
            adItem.onClick.AddListener(() =>
            {
                AdManager.Instance.RewardedAd.OnAdClosed += OnAdClosedHandler;
                AdManager.Instance.ShowAds();
            });
        }
    }
}
