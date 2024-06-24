using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitLevelUpButtonUI : UIBase
{
    Button _levelUp;


    protected override void Awake()
    {
        base.Awake();
        _levelUp = transform.Find("Button - LevelUpButton").GetComponent<Button>();
    }

    void Start()
    {
        _levelUp.onClick.AddListener(() => UIManager.Instance.Get<UnitLevelUpUI>().Show());
    }
}
