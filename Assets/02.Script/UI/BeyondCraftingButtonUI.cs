using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeyondCraftingButtonUI : UIBase
{
    Button _beyondCrafting;

    protected override void Awake()
    {
        base.Awake();
        _beyondCrafting = transform.Find("Button - BeyondCraftingButton").GetComponent<Button>();
        _beyondCrafting.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _beyondCrafting.interactable = false;
        GameManager.Instance.OnGameStart += () =>
        {
            _beyondCrafting.interactable = true;
        };
        GameManager.Instance.OnGameEnd += v =>
        {
            _beyondCrafting.interactable = false;
        };
    }

    private void Start()
    {
        _beyondCrafting.onClick.AddListener(() => UIManager.Instance.Get<BeyondCraftingUI>().Show());
    }
}
