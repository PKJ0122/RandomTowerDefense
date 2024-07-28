using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSettingUI : UIBase
{
    const int WEIGHT = 10;
    const string EMAIL = "qlrrudwns@gmail.com";

#if UNITY_EDITOR
    static string s_filePath = "/../PlayerSetting.json";
#else
    static string s_filePath = "/storage/emulated/0/Android/data/com.KJParkCompany.DragonDefense/files/PlayerSetting.json";
#endif

    static PlayerSetting s_playerSetting;
    public static PlayerSetting PlayerSetting
    {
        get
        {
            if (s_playerSetting == null)
            {
                if (File.Exists(s_filePath))
                {
                    string json = File.ReadAllText(s_filePath);
                    if (json == string.Empty) return s_playerSetting = new PlayerSetting();

                    s_playerSetting = JsonUtility.FromJson<PlayerSetting>(json);
                }
                else
                    return s_playerSetting = new PlayerSetting();
            }
            return s_playerSetting;
        }
    }


    Slider _master;
    Slider _bgm;
    Slider _sfx;
    Toggle _masterMute;
    Toggle _bgmMute;
    Toggle _sfxMute;
    Slider _frame;
    TMP_Text _frameT;
    Button _emailCopy;
    Button _tutorial;
    Toggle _vibration;
    Button _close;


    protected override void Awake()
    {
        base.Awake();
        _master = transform.Find("Panel/Image/Slider - Master").GetComponent<Slider>();
        _bgm = transform.Find("Panel/Image/Slider - BGM").GetComponent<Slider>();
        _sfx = transform.Find("Panel/Image/Slider - SFX").GetComponent<Slider>();
        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _masterMute = _master.transform.Find("Toggle - Mute").GetComponent<Toggle>();
        _bgmMute = _bgm.transform.Find("Toggle - Mute").GetComponent<Toggle>();
        _sfxMute = _sfx.transform.Find("Toggle - Mute").GetComponent<Toggle>();
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
        _frameT = transform.Find("Panel/Image/Slider - Frame/Text (TMP) - Amount").GetComponent<TMP_Text>();
        _frame.value = PlayerSetting.frame;
        _frameT.text = (PlayerSetting.frame * WEIGHT).ToString();
        _frame.onValueChanged.AddListener(value =>
        {
            PlayerSetting.frame = (int)value;
            _frameT.text = (value * WEIGHT).ToString();
            Application.targetFrameRate = (int)value * WEIGHT;
        });

        _vibration = transform.Find("Panel/Image/Toggle - Vibration").GetComponent<Toggle>();
        _vibration.isOn = PlayerSetting.vibration;
        _vibration.onValueChanged.AddListener(value =>
        {
            PlayerSetting.vibration = value;
        });

        _emailCopy = transform.Find("Panel/Image/Text (TMP) - Email/Button - Copy").GetComponent<Button>();
        _emailCopy.onClick.AddListener(() => GUIUtility.systemCopyBuffer = EMAIL);

        _tutorial = transform.Find("Panel/Image/Button - Tutorial").GetComponent<Button>();
        _tutorial.onClick.AddListener(() => SceneManager.LoadScene("Tutorial"));
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        return;
#endif

        string json = JsonUtility.ToJson(s_playerSetting);
        File.WriteAllText(s_filePath, json);
    }

    public override void Show()
    {
        base.Show();
        SortingOrder = 999;
    }
}
