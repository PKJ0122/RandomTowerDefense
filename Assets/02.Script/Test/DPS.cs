using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DPS : MonoBehaviour
{
    public UnitBase _unit;
    float _tick;
    public TMP_Text _text;
    public Button _button;

    void Awake()
    {
        _text = transform.Find("Text (TMP)").GetComponent<TMP_Text>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => UIManager.Instance.Get<UnitInfoUI>().Show(_unit.Slot));
    }

    private void Update()
    {
        _tick += Time.deltaTime;
        _text.text = $"{Math.Floor(_unit.Damage / _tick)}";
    }
}
