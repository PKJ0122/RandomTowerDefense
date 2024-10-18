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
        PlayerData.Instance.OnItemSummonsChange += OnItemSummonsChangeHandler;
        PlayerData.Instance.OnLastShopChangeChange += ShopRefresh;
        _itemSummons = transform.Find("Panel - Summons/Text (TMP) - Summons").GetComponent<TMP_Text>();
        _itemSummons.text = PlayerData.Instance.ItemSummons.ToString("N0");

        Button itemSummonsButton = Slots[1].GetComponent<Button>();
        itemSummonsButton.onClick.AddListener(() =>
        {
            if (PlayerData.Instance.ItemSummons < 1) return;

            PlayerData.Instance.ItemSummons--;
            UIManager.Instance.Get<ItemDrawUI>().Show();
        });
        itemSummonsButton.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        Button diamondButton = Slots[2].GetComponent<Button>();
        diamondButton.onClick.AddListener(() =>
        {
            if (PlayerData.Instance.Diamond < PRICE) return;

            PlayerData.Instance.Diamond -= PRICE;
            UIManager.Instance.Get<ItemDrawUI>().Show();
        });
        diamondButton.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
    }

    void Start()
    {
        OnAdClosedHandler += (object sender, EventArgs args) =>
        {
            UIManager.Instance.Get<ItemDrawUI>().Show();
            PlayerData.PlayerDataContainer.adItemBuy = true;
            ShopRefresh();
            AdManager.Instance.RewardedAd.OnAdClosed -= OnAdClosedHandler;
        };
    }

    protected override void ShopRefresh()
    {
        Slots[0].Find("Panel - Buy").gameObject.SetActive(PlayerData.PlayerDataContainer.adItemBuy);
        if (!PlayerData.PlayerDataContainer.adItemBuy)
        {
            Button adItem = Slots[0].GetComponent<Button>();
            adItem.onClick.RemoveAllListeners();
            adItem.onClick.AddListener(() =>
            {
                AdManager.Instance.RewardedAd.OnAdClosed += OnAdClosedHandler;
                AdManager.Instance.ShowAds();
                SoundManager.Instance.PlaySound(SFX.Button_Click);
                adItem.onClick.RemoveAllListeners();
            });
        }
    }
}
