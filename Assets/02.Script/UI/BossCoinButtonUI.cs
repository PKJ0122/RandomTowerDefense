using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCoinButtonUI : UIBase
{
    Button _bossCoin;
    Image _isUsePossible;


    protected override void Awake()
    {
        base.Awake();
        _bossCoin = transform.Find("Button - BossCoinButton").GetComponent<Button>();
        _isUsePossible = _bossCoin.transform.Find("Image - IsUsePossible").GetComponent<Image>();
        _bossCoin.interactable = false;
        _bossCoin.onClick.AddListener(() => UIManager.Instance.Get<BossCoinUI>().Show());
        _bossCoin.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        GameManager.Instance.OnGameStart += () =>
        {
            _bossCoin.interactable = true;
        };
        GameManager.Instance.OnGameEnd += v =>
        {
            _bossCoin.interactable = false;
        };
        GameManager.Instance.OnKeyChange += v =>
        {
            _isUsePossible.enabled = v != 0;
        };
    }
}
