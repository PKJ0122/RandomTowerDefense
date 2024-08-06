using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectSpawnUI : UIBase
{
    Button _unitPrefeb;

    Transform _location;
    TMP_Text _selectRankT;

    UnitRank _selectUnitRank;

    Button[] _units = new Button[3];

    public event Action<UnitBase> OnIsUse;


    protected override void Awake()
    {
        base.Awake();
        _unitPrefeb = Resources.Load<Button>("Button - UnitSelectButton");
        _location = transform.Find("Panel/Image").GetComponent<Transform>();
        _selectRankT = transform.Find("Text (TMP) - SelectRank").GetComponent<TMP_Text>();

        for (int i = 0; i < _units.Length; i++)
        {
            _units[i] = Instantiate(_unitPrefeb, _location);
        }
    }

    public void Show(UnitRank unitRank,Slot slot)
    {
        base.Show();
        
        if (unitRank != _selectUnitRank)
        {
            _selectUnitRank = unitRank;
            UnitRankData unitRankData = UnitRepository.UnitRankDatas[unitRank];
            string color = unitRankData.unitRankColor.ToHexString();

            string name = unitRankData.unitRankName;

            _selectRankT.text = $"선택한   <i><size=80><b><color=#{color}>{name}</i></size></color></b> 등급의 유닛을 소환합니다";
        }

        Dictionary<UnitKind, UnitData> unitDatas = UnitRepository.UnitKindDatas;
        foreach (Button item in _units)
        {
            Image buttonImg = item.transform.Find("Image - Unit").GetComponent<Image>();

            UnitKind unitKind = UnitFactory.Instance.RandomUnitKind();
            buttonImg.sprite = unitDatas[unitKind].unitImg;

            item.onClick.RemoveAllListeners();
            item.onClick.AddListener(() =>
            {
                UnitBase unit = UnitFactory.Instance.UnitCreat<UnitBase>(slot, unitKind, _selectUnitRank);
                OnIsUse?.Invoke(unit);
                Hide();
            });
        }
    }
}