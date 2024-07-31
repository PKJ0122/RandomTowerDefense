using UnityEngine.UI;

public class UnitLevelUpButtonUI : UIBase
{
    Button _levelUp;


    protected override void Awake()
    {
        base.Awake();
        _levelUp = transform.Find("Button - LevelUpButton").GetComponent<Button>();
        _levelUp.interactable = false;
        GameManager.Instance.OnGameStart += () =>
        {
            _levelUp.interactable = true;
        };
        GameManager.Instance.OnGameEnd += v =>
        {
            _levelUp.interactable = false;
        };
    }

    void Start()
    {
        _levelUp.onClick.AddListener(() => UIManager.Instance.Get<UnitLevelUpUI>().Show());
    }
}
