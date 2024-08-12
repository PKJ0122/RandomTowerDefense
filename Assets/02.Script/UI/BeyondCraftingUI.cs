using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeyondCraftingUI : UIBase
{
    Button _beyondSlotPrefab;
    Transform _location;
    RectTransform _content;
    Transform _info;
    Button _beyond;
    Button _close;

    Image _unit;
    Image[] _materials = new Image[3];

    BeyondCraftingCounter _currentCounter;

    public event Action OnBeyond;


    protected override void Awake()
    {
        base.Awake();
        _beyondSlotPrefab = Resources.Load<Button>("Button - BeyondSlot");
        _info = transform.Find("Panel/Image/Image - Info").GetComponent<Transform>();
        _location = transform.Find("Panel/Image/Scroll View/Viewport/Content").GetComponent<Transform>();
        _content = _location.GetComponent<RectTransform>();
        _unit = _info.Find("Image - Unit").GetComponent<Image>();
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i] = _info.Find($"Image - Material{i}").GetComponent<Image>();
        }
        _beyond = _info.Find("Button - Beyond").GetComponent<Button>();
        _beyond.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _beyond.onClick.AddListener(Beyond);
        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _close.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _close.onClick.AddListener(Hide);

        BeyondCraftingCounterManager.Instance.OnCounterCreate += counter =>
        {
            Button beyondInfo = Instantiate(_beyondSlotPrefab, _location);
            Image unitImg = beyondInfo.transform.Find("Image - Unit").GetComponent<Image>();
            Image creatPossible = beyondInfo.transform.Find("Image - Creat").GetComponent<Image>();

            UnitData unitData = UnitRepository.UnitKindDatas[counter.Method.unitKind];
            unitImg.sprite = unitData.unitImg;

            beyondInfo.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
            beyondInfo.onClick.AddListener(() =>
            {
                _currentCounter = counter;
                _info.gameObject.SetActive(true);
                Refresh();
            });
            counter.OnBeyondCraftingPossibleChange += value =>
            {
                creatPossible.gameObject.SetActive(value);
                if (value)
                {
                    beyondInfo.transform.SetAsFirstSibling();
                }
                else
                {
                    beyondInfo.transform.SetAsLastSibling();
                }
            };
        };
    }

    public override void Show()
    {
        base.Show();
        _content.anchoredPosition = Vector2.zero;
        _info.gameObject.SetActive(false);
        _currentCounter = null;
    }

    void Refresh()
    {
        _unit.sprite = UnitRepository.UnitKindDatas[_currentCounter.Method.unitKind].unitImg;
        _beyond.interactable = _currentCounter.IsBeyondCraftingPossible;

        for (int i = 0; i < _materials.Length; i++)
        {
            Image unitImg = _materials[i].transform.Find("Image - Unit").GetComponent<Image>();
            TMP_Text unitRank = _materials[i].transform.Find("Text (TMP) - Rank").GetComponent<TMP_Text>();
            TMP_Text ishave = _materials[i].transform.Find("Text (TMP) - IsHave").GetComponent<TMP_Text>();

            UnitData unitData = UnitRepository.UnitKindDatas[_currentCounter.Method.beyondCraftingMaterials[i].unitKind];
            unitImg.sprite = unitData.unitImg;
            UnitRankData rankData = UnitRepository.UnitRankDatas[_currentCounter.Method.beyondCraftingMaterials[i].unitRank];
            unitRank.text = rankData.unitRankName;
            unitRank.color = rankData.unitRankColor;
            ishave.gameObject.SetActive(_currentCounter.Materials[i].Count > 0);
        }
    }

    void Beyond()
    {
        if (!_currentCounter.IsBeyondCraftingPossible) return;

        Slot slot = _currentCounter.Materials[0][0].Slot;

        for (int i = 0; i < _materials.Length; i++)
        {
            List<UnitBase> materials = _currentCounter.Materials[i];

            materials[materials.Count - 1].RelasePool();
        }

        UnitKind Kind = _currentCounter.Method.unitKind;

        UnitFactory.Instance.UnitCreat<BeyondBase>(slot, Kind, UnitRank.Beyond);

        Hide();
    }
}