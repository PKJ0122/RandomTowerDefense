using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUI : UIBase
{
    ItemDatas _itemDatas;
    TMP_Text _name;
    TMP_Text _discription;
    TMP_Text _level;
    Slider _itemCounts;
    TMP_Text _itemCountT;
    Image _itemImg;
    Button _levelUp;
    Button _close;

    Action<ItemLevelData> OnItemDataChangeHandler;


    protected override void Awake()
    {
        base.Awake();
        _itemDatas = Resources.Load<ItemDatas>("ItemDatas");
        _name = transform.Find("Panel/Image/Image - Name/Text (TMP)").GetComponent<TMP_Text>();
        _discription = transform.Find("Panel/Image/Image - Discription/Text (TMP)").GetComponent<TMP_Text>();
        _itemImg = transform.Find("Panel/Image/Image - Item/Image").GetComponent<Image>();
        _levelUp = transform.Find("Panel/Image/GameObject/Button - LevelUp").GetComponent<Button>();
        _close = transform.Find("Panel/Image/GameObject/Button - Close").GetComponent<Button>();
        _close.onClick.AddListener(Hide);
        _close.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _level = transform.Find("Panel/Image/Text (TMP) - ItemLevel").GetComponent<TMP_Text>();
        _itemCounts = transform.Find("Panel/Image/Slider - Count").GetComponent<Slider>();
        _itemCountT = _itemCounts.transform.Find("Text (TMP) - Count").GetComponent<TMP_Text>();

        OnItemDataChangeHandler += itemLevelData =>
        {
            _levelUp.interactable = IsLevelUpPossible(itemLevelData.itemName);
        };
    }

    private void Start()
    {
        _levelUp.onClick.AddListener(Hide);
    }

    public void Show(string itemName)
    {
        _levelUp.gameObject.SetActive(UIManager.Instance.UIPeekCheck(UIManager.Instance.Get<InventoryUI>()));
        _levelUp.interactable = IsLevelUpPossible(itemName);
        _levelUp.onClick.RemoveAllListeners();
        _levelUp.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _levelUp.onClick.AddListener(() =>
        {
            ItemLevelUpUI levelUpUI = UIManager.Instance.Get<ItemLevelUpUI>();
            levelUpUI.Show(itemName);
            PlayerData.Instance.OnItemDataChange += OnItemDataChangeHandler;
        });
        base.Show();
        ItemBase itemBase = _itemDatas.Items[itemName];
        _name.text = itemBase.itemName;
        _discription.text = $"{itemBase.Description}";
        _itemImg.sprite = itemBase.itemImage;

        _itemImg.color = PlayerData.Instance.IsItemPossess(itemName) ? Color.white : Color.black;
    }

    public override void Hide()
    {
        base.Hide();
        PlayerData.Instance.OnItemDataChange -= OnItemDataChangeHandler;
    }

    public bool IsLevelUpPossible(string itemName)
    {
        if (!PlayerData.ItemLevels.TryGetValue(itemName, out ItemLevelData itemLevelData))
        {
            _levelUp.gameObject.SetActive(false);
            _level.text = $"0 Lv";
            _itemCounts.maxValue = 1;
            _itemCounts.value = 0;
            _itemCountT.text = $"0 / 1";
            return false;
        }

        int level = Math.Min(9,itemLevelData.level);
        int nowAmount = itemLevelData.Amount;
        int upNeedAmount = _itemDatas.itemLevelUpNeedAmount[level];

        _level.text = $"{itemLevelData.level} Lv";
        _itemCounts.maxValue = upNeedAmount;
        _itemCounts.value = nowAmount;
        _itemCountT.text = $"{nowAmount} / {upNeedAmount}";

        return nowAmount >= upNeedAmount;
    }
}
