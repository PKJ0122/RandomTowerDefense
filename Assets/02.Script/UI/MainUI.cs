using System;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : UIBase
{
    TMP_Text _playerName;
    Button _playerNameChange;

    Button _shop;
    Button _inventory;
    Button _gameStart;
    Button _playerSetting;

    Action<string> _onPlayerNameChangeHandler;


    protected override void Awake()
    {
        base.Awake();
        _playerName = transform.Find("Image - Profile/Button - Name/Text (TMP)").GetComponent<TMP_Text>();
        _playerNameChange = transform.Find("Image - Profile/Button - Name").GetComponent<Button>();
        _shop = transform.Find("Button - Shop").GetComponent<Button>();
        _inventory = transform.Find("Button - Inventory").GetComponent<Button>();
        _playerSetting = transform.Find("Button - Setting").GetComponent<Button>();
        _gameStart = transform.Find("Button - GameStart").GetComponent<Button>();
        _gameStart.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        _onPlayerNameChangeHandler += value =>
        {
            _playerName.text = value;
        };
    }

    void Start()
    {
        _playerNameChange.onClick.AddListener(() => UIManager.Instance.Get<PlayerNameUI>().Show());
        _playerName.text = PlayerData.Instance.PlayerName;
        _shop.onClick.AddListener(() => UIManager.Instance.Get<ShopUI>().Show());
        _inventory.onClick.AddListener(() => UIManager.Instance.Get<InventoryUI>().Show());
        _playerSetting.onClick.AddListener(() => UIManager.Instance.Get<PlayerSettingUI>().Show());
        PlayerData.OnPlayerNameChange += _onPlayerNameChangeHandler;
    }

    void OnDestroy()
    {
        PlayerData.OnPlayerNameChange -= _onPlayerNameChangeHandler;
    }
}
