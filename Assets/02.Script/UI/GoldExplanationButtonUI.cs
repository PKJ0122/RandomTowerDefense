using UnityEngine.UI;

public class GoldExplanationButtonUI : UIBase
{
    Button _goldExplanation;


    protected override void Awake()
    {
        base.Awake();
        _goldExplanation = transform.Find("Button - UnitBuyInfo").GetComponent<Button>();
    }

    void Start()
    {
        _goldExplanation.onClick.AddListener(() => UIManager.Instance.Get<GoldExplanationUI>().Show());
    }
}