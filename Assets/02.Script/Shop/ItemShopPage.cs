using System;
using TMPro;
using UnityEngine.UI;

public class ItemShopPage : ShopPageBase
{
    const int PRICE = 500;

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
        PlayerData.OnItemSummonsChange += OnItemSummonsChangeHandler;
        PlayerData.OnLastShopChangeChange += ShopRefresh;
        _itemSummons = transform.Find("Panel - Summons/Text (TMP) - Summons").GetComponent<TMP_Text>();
        _itemSummons.text = PlayerData.Instance.ItemSummons.ToString("N0");

        Button itemSummonsButton = Slots[1].GetComponent<Button>();
        itemSummonsButton.onClick.AddListener(() =>
        {
            if (PlayerData.Instance.ItemSummons < 1) return;

            PlayerData.Instance.ItemSummons--;
            UIManager.Instance.Get<ItemDrawUI>().Show();
        });
        Button DiamondButton = Slots[2].GetComponent<Button>();
        DiamondButton.onClick.AddListener(() =>
        {
            if (PlayerData.Instance.Diamond < PRICE) return;

            PlayerData.Instance.Diamond -= PRICE;
            UIManager.Instance.Get<ItemDrawUI>().Show();
        });
    }

    void Start()
    {
        OnAdClosedHandler += (object sender, EventArgs args) =>
        {
            UIManager.Instance.Get<ItemDrawUI>().Show();
            PlayerData.Instance.PlayerDataContainer.adItemBuy = true;
            ShopRefresh();
            AdManager.Instance.RewardedAd.OnAdClosed -= OnAdClosedHandler;
        };
    }

    void OnDestroy()
    {
        PlayerData.OnItemSummonsChange -= OnItemSummonsChangeHandler;
        PlayerData.OnLastShopChangeChange -= ShopRefresh;
    }

    protected override void ShopRefresh()
    {
        Slots[0].Find("Panel - Buy").gameObject.SetActive(PlayerData.Instance.PlayerDataContainer.adItemBuy);
        if (!PlayerData.Instance.PlayerDataContainer.adItemBuy)
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
