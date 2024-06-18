using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    static Dictionary<Slot, UnitBase> s_slots;
    static public Dictionary<Slot,UnitBase> Slots
    {
        get
        {
            if (s_slots == null)
                s_slots = new Dictionary<Slot, UnitBase>(81);

            return s_slots;
        }
    }

    LayerMask _layerMank;


    void Awake()
    {
        _layerMank = LayerMask.GetMask("Slot");
    }

    void OnEnable()
    {
        UIManager.Instance.Get<UnitBuyUI>().onBuyButtonClick += () => IsVacancy();
        UIManager.Instance.Get<UnitBuyUI>().onUnitBuySuccess += (slot, unit) =>
        {
            Slots[slot] = unit;
            unit.gameObject.transform.position = slot.gameObject.transform.position;
        };
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _layerMank) &&
                    Slots[hit.collider.GetComponent<Slot>()] != null)
                {
                    UIManager.Instance.Get<UnitInfoUI>().Show(Slots[hit.collider.GetComponent<Slot>()]);
                }
            }
        }
    }

    //void OnDisable()
    //{
    //    UIManager.Instance.Get<UnitBuyUI>().onBuyButtonClick -= () => IsVacancy();
    //    UIManager.Instance.Get<UnitBuyUI>().onUnitBuySuccess -= (slot, unit) =>
    //    {
    //        Slots[slot] = unit;
    //        unit.gameObject.transform.position = slot.gameObject.transform.position;
    //    };
    //}

    /// <summary>
    /// ������ ������ ������ �ִ��� Ȯ���ϰ� �ִٸ� �ش� Slot�� ��ȯ���ִ� �Լ� / ���ڸ��� ���ٸ� null�� ��ȯ
    /// </summary>
    Slot IsVacancy()
    {
        foreach (KeyValuePair<Slot,UnitBase> item in Slots)
        {
            if (item.Value == null)
            {
                return item.Key;
            }
        }

        return null;
    }
}