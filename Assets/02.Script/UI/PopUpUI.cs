using TMPro;
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
    }

    public void Show(string detail)
    {
        base.Show();
        SortingOrder = 99999;
        _detail.text = detail;
    }
}