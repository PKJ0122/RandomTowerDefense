using TMPro;
using UnityEngine.UI;

public class MainUI : UIBase
{
    TMP_Text _playerName;
    Button _playerNameChange;


    protected override void Awake()
    {
        base.Awake();
        _playerName = transform.Find("Image - Profile/Button - Name/Text (TMP)").GetComponent<TMP_Text>();
        _playerNameChange = transform.Find("Image - Profile/Button - Name").GetComponent<Button>();
    }

    private void Start()
    {
        _playerNameChange.onClick.AddListener(() => UIManager.Instance.Get<PlayerNameUI>().Show());
        _playerName.text = PlayerData.Instance.PlayerName;
        PlayerData.Instance.OnPlayerNameChange += value =>
        {
            _playerName.text = PlayerData.Instance.PlayerName;
        };
    }
}
