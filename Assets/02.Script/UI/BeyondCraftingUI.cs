using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeyondCraftingUI : UIBase
{
    BeyondCraftingMethods _beyondCraftingMethods;
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
        _beyondCraftingMethods = Resources.Load<BeyondCraftingMethods>("BeyondCraftingMethods");
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
        _beyond.onClick.AddListener(Beyond);
        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _close.onClick.AddListener(Hide);
    }

    private void Start()
    {
        foreach (var data in _beyondCraftingMethods.beyondCraftingMethods)
        {
            PlayerData.beyondCraftingDatas.Add(data.unitKind, new BeyondCraftingData
            {
                unitKind = data.unitKind,
                IsHave = true
            });
        }
        foreach (KeyValuePair<UnitKind, BeyondCraftingData> item in PlayerData.beyondCraftingDatas)
        {
            Button beyondInfo = Instantiate(_beyondSlotPrefab, _location);
            Image unitImg = beyondInfo.transform.Find("Image - Unit").GetComponent<Image>();
            Image creatPossible = beyondInfo.transform.Find("Image - Unit").GetComponent<Image>();
            BeyondCraftingCounter counter =
                new BeyondCraftingCounter(_beyondCraftingMethods.BeyondCraftingMethodDatas[item.Key]);

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

            unitImg.sprite = UnitRepository.UnitKindDatas[item.Key].unitImg;
        }
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

        Slot slot = null;

        for (int i = 0; i < _materials.Length; i++)
        {
            List<UnitBase> materials = _currentCounter.Materials[i];

            if (i == 0) slot = materials[materials.Count - 1].Slot;

            materials[materials.Count - 1].RelasePool();
            materials.RemoveAt(materials.Count - 1);
        }

        UnitKind unitKind = _currentCounter.Method.unitKind;

        UnitBase beyond = ObjectPoolManager.Instance.Get($"Beyond_{unitKind}")
                                                    .Get()
                                                    .GetComponent<BeyondBase>()
                                                    .UnitSet(slot, unitKind, UnitRank.Beyond);

        OnBeyond?.Invoke();
        Refresh();
    }
}