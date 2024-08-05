using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCoinButtonUI : UIBase
{
    Button _bossCoin;


    protected override void Awake()
    {
        base.Awake();
        _bossCoin = transform.Find("Button - BossCoinButton").GetComponent<Button>();
        _bossCoin.interactable = false;
        _bossCoin.onClick.AddListener(() => UIManager.Instance.Get<BossCoinUI>().Show());
        GameManager.Instance.OnGameStart += () =>
        {
            _bossCoin.interactable = true;
        };
        GameManager.Instance.OnGameEnd += v =>
        {
            _bossCoin.interactable = false;
        };
    }
}
