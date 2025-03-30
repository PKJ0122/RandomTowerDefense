using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class PopUpUI : UIBase
{
    TMP_Text _detail;
    Button _ok;

    protected override void Awake()
    {
        base.Awake();
        _detail = transform.Find("Panel/Image/Text (TMP) - Detail").GetComponent<TMP_Text>();
        _ok = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _ok.onClick.AddListener(Hide);
        _ok.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
    }

    public void Show(string detail)
    {
        base.Show();
        SortingOrder = 999;
        _detail.text = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", detail, LocalizationSettings.SelectedLocale);
    }
}