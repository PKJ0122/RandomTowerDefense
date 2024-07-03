using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawnUI : UIBase
{
    public TMP_Dropdown kind;
    public TMP_Dropdown rank;
    public Button spawn;
    public Button hide;

    Slot _slot;
    LayerMask _layerMank;

    public event Action<UnitBase> OnUnitSpawn;


    protected override void Awake()
    {
        base.Awake();
        List<TMP_Dropdown.OptionData> kindList = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < System.Enum.GetValues(typeof(UnitKind)).Length; i++)
        {
            TMP_Dropdown.OptionData z = new TMP_Dropdown.OptionData();
            z.text = ((UnitKind)i).ToString();
            kindList.Add(z);
        }
        kind.options = kindList;
        List<TMP_Dropdown.OptionData> rankList = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < System.Enum.GetValues(typeof(UnitRank)).Length; i++)
        {
            TMP_Dropdown.OptionData z = new TMP_Dropdown.OptionData();
            z.text = ((UnitRank)i).ToString();
            rankList.Add(z);
        }
        rank.options = rankList;

        spawn.onClick.AddListener(() =>
        {
            OnUnitSpawn?.Invoke(ObjectPoolManager.Instance.Get(((UnitKind)kind.value).ToString())
                                                          .Get()
                                                          .GetComponent<UnitBase>()
                                                          .UnitSet(_slot, (UnitKind)kind.value, (UnitRank)rank.value));
            Hide();
        });
        hide.onClick.AddListener(Hide);
        _layerMank = LayerMask.GetMask("Slot");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _layerMank))
            {
                _slot = hit.collider.GetComponent<Slot>();
                Show();
            }
        }
    }
}
