using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameUI : UIBase
{
    TMP_InputField _inputField;
    Button _ok;
    Button _close;

    protected override void Awake()
    {
        base.Awake();
        _inputField = transform.Find("Panel/Image/InputField (TMP)").GetComponent<TMP_InputField>();
        _ok = transform.Find("Panel/Image/Button - Ok").GetComponent<Button>();
        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _inputField.onEndEdit.AddListener(value => _ok.interactable = NameChack(value));
        _ok.onClick.AddListener(() =>
        {
            PlayerData.Instance.PlayerName = _inputField.text;
            Hide();
        });
        _close.onClick.AddListener(Hide);
    }

    public override void Show()
    {
        base.Show();
        _inputField.text = "";
        _ok.interactable = false;
    }

    bool NameChack(string text)
    {
        if (2 <= text.Length && text.Length <= 6) return true;

        return false;
    }
}
