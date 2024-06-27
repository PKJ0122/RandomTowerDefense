using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionButtonUI : UIBase
{
    Button _mission;


    protected override void Awake()
    {
        base.Awake();
        _mission = transform.Find("Button - MissionButton").GetComponent<Button>();
    }

    private void Start()
    {
        _mission.onClick.AddListener(() => UIManager.Instance.Get<MissionUI>().Show());
    }
}
