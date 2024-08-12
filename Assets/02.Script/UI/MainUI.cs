using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : UIBase
{
    Button _shop;
    Button _inventory;
    Button _beyondCrafting;
    Button _quest;
    Button _mailBox;
    Button _playerSetting;
    Button _gameStart;

    Action<string> _onPlayerNameChangeHandler;


    protected override void Awake()
    {
        base.Awake();
        _shop = transform.Find("Button - Shop").GetComponent<Button>();
        _shop.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _inventory = transform.Find("Button - Inventory").GetComponent<Button>();
        _inventory.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _beyondCrafting = transform.Find("Button - BeyondCrafting").GetComponent<Button>();
        _beyondCrafting.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _quest = transform.Find("Button - Quest").GetComponent<Button>();
        _quest.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _mailBox = transform.Find("Button - Mailbox").GetComponent<Button>();
        _mailBox.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _playerSetting = transform.Find("Button - Setting").GetComponent<Button>();
        _playerSetting.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _gameStart = transform.Find("Button - GameStart").GetComponent<Button>();
        _gameStart.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _gameStart.onClick.AddListener(() => SceneManager.LoadScene("Game"));

        SoundManager.Instance.PlaySound(BGM.Lobby);
    }

    void Start()
    {
        _shop.onClick.AddListener(() => UIManager.Instance.Get<ShopUI>().Show());
        _inventory.onClick.AddListener(() => UIManager.Instance.Get<InventoryUI>().Show());
        _beyondCrafting.onClick.AddListener(() => UIManager.Instance.Get<BeyondCraftingShopUI>().Show());
        _quest.onClick.AddListener(() => UIManager.Instance.Get<QuestUI>().Show());
        _mailBox.onClick.AddListener(() => UIManager.Instance.Get<MailBoxUI>().Show());
        _playerSetting.onClick.AddListener(() => UIManager.Instance.Get<PlayerSettingUI>().Show());
        PlayerData.Instance.OnPlayerNameChange += _onPlayerNameChangeHandler;
    }
}
