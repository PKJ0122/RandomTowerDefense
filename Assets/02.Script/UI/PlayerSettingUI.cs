using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSettingUI : UIBase
{
    const int WEIGHT = 10;
    const string EMAIL = "qlrrudwns@gmail.com";

    static PlayerSetting s_playerSetting;
    public PlayerSetting PlayerSetting
    {
        get
        {
            if (s_playerSetting == null)
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    s_playerSetting = JsonUtility.FromJson<PlayerSetting>(json);
                }
                else
                    return s_playerSetting = new PlayerSetting();
            }
            return s_playerSetting;
        }
    }

    string _filePath;

    Slider _master;
    Slider _bgm;
    Slider _sfx;
    Toggle _masterMute;
    Toggle _bgmMute;
    Toggle _sfxMute;
    Slider _frame;
    Button _emailCopy;
    Button _tutorial;
    Button _close;


    protected override void Awake()
    {
        base.Awake();
        _filePath = Path.Combine(Application.persistentDataPath, "PlayerSetting.json");
        _master = transform.Find("Panel/Image/Slider - Master").GetComponent<Slider>();
        _bgm = transform.Find("Panel/Image/Slider - BGM").GetComponent<Slider>();
        _sfx = transform.Find("Panel/Image/Slider - SFX").GetComponent<Slider>();
        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _masterMute = _master.transform.Find("Toggle - Mute").GetComponent<Toggle>();
        _bgmMute = _master.transform.Find("Toggle - Mute").GetComponent<Toggle>();
        _sfxMute = _master.transform.Find("Toggle - Mute").GetComponent<Toggle>();
        _master.value = PlayerSetting.masterVolume;
        _bgm.value = PlayerSetting.bgmVolume;
        _sfx.value = PlayerSetting.sfxVolume;
        _masterMute.isOn = PlayerSetting.masterMute;
        _bgmMute.isOn = PlayerSetting.bgmMute;
        _sfxMute.isOn = PlayerSetting.sfxMute;
        _master.onValueChanged.AddListener(value =>
        {
            PlayerSetting.masterVolume = value;
        });
        _bgm.onValueChanged.AddListener(value =>
        {
            PlayerSetting.bgmVolume = value;
        });
        _sfx.onValueChanged.AddListener(value =>
        {
            PlayerSetting.sfxVolume = value;
        });
        _masterMute.onValueChanged.AddListener(value =>
        {
            PlayerSetting.masterMute = value;
        });
        _bgmMute.onValueChanged.AddListener(value =>
        {
            PlayerSetting.bgmMute = value;
        });
        _sfxMute.onValueChanged.AddListener(value =>
        {
            PlayerSetting.sfxMute = value;
        });

        _close.onClick.AddListener(Hide);

        Application.targetFrameRate = PlayerSetting.frame * WEIGHT;
        _frame = transform.Find("Panel/Image/Slider - Frame").GetComponent<Slider>();
        _frame.value = PlayerSetting.frame*WEIGHT;
        _frame.onValueChanged.AddListener(value =>
        {
            PlayerSetting.frame = (int)value;
            Application.targetFrameRate = (int)value * WEIGHT;
        });

        _emailCopy = transform.Find("Panel/Image/Text (TMP) - Email/Button - Copy").GetComponent<Button>();
        _emailCopy.onClick.AddListener(() => GUIUtility.systemCopyBuffer = EMAIL);

        _tutorial.onClick.AddListener(() => SceneManager.LoadScene("Tutorial"));
    }

    private void OnApplicationQuit()
    {
        string json = JsonUtility.ToJson(s_playerSetting);
        File.WriteAllText(_filePath, json);
    }

    public override void Show()
    {
        base.Show();
        SortingOrder = 999;
    }
}
