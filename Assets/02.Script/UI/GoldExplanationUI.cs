using UnityEngine.UI;

public class GoldExplanationUI : UIBase
{
    Button _close;


    protected override void Awake()
    {
        base.Awake();
        _close = transform.Find("Panel/Panel/Button - CloseButton").GetComponent<Button>();
        _close.onClick.AddListener(Hide);
    }
}