using TMPro;
using UnityEngine.UI;

public class GoldExplanationUI : UIBase
{
    TMP_Text _Interest;
    TMP_Text _Salary;

    Button _close;


    protected override void Awake()
    {
        base.Awake();
        _close = transform.Find("Panel/Panel/Button - CloseButton").GetComponent<Button>();
        _close.onClick.AddListener(Hide);
        _Interest = transform.Find("Panel/Panel/Text (TMP) - Interest").GetComponent<TMP_Text>();
        _Salary = transform.Find("Panel/Panel/Text (TMP) - Salary").GetComponent<TMP_Text>();

        GameManager.Instance.OnGameStart += () =>
        {
            _Interest.text = $"+ {GameManager.Instance.Interest}% Gold";
            _Salary.text = $"+ {GameManager.Instance.Salary} Gold";
        };
    }
}