using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : UIBase
{
    float _beforeTimeScale;
    Button _resume;
    Button _giveUp;


    protected override void Awake()
    {
        base.Awake();
        _resume = transform.Find("Panel/Image/Button - Resume").GetComponent<Button>();
        _giveUp = transform.Find("Panel/Image/Button - GiveUp").GetComponent<Button>();
        _resume.onClick.AddListener(() =>
        {
            Hide();
            Time.timeScale = _beforeTimeScale;
        });
        _giveUp.onClick.AddListener(() =>
        {
            Hide();
            GameManager.Instance.GameEnd();
        });
    }

    public override void Show()
    {
        base.Show();
        _beforeTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }
}
