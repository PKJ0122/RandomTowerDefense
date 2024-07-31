using UnityEngine.UI;

public class UnitBuyInfoButtonUI : UIBase
{
    Button _unitBuyInfo;


    protected override void Awake()
    {
        base.Awake();
        _unitBuyInfo = transform.Find("Button - UnitBuyInfo").GetComponent<Button>();


        _unitBuyInfo.interactable = false;
        GameManager.Instance.OnGameStart += () => _unitBuyInfo.interactable = true;
    }

    private void Start()
    {
        _unitBuyInfo.onClick.AddListener(() =>
        {
            UIManager.Instance.Get<UnitBuyInfoUI>().Show();
        });
    }
}
