using UnityEngine.UI;

public class GameSettingUI : UIBase
{
    Button _setting;
    Button _pause;

    protected override void Awake()
    {
        base.Awake();
        _setting = transform.Find("Button - Setting").GetComponent<Button>();
        _setting.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _pause = transform.Find("Button - Pause").GetComponent<Button>();
        _pause.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
    }

    void Start()
    {
        _setting.onClick.AddListener(() => UIManager.Instance.Get<PlayerSettingUI>().Show());
        _pause.onClick.AddListener(() => UIManager.Instance.Get<PauseUI>().Show());
    }
}
