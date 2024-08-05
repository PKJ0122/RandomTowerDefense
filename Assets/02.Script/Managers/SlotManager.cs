using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    static Dictionary<Slot, UnitBase> s_slots;
    static public Dictionary<Slot, UnitBase> Slots
    {
        get
        {
            if (s_slots == null)
                s_slots = new Dictionary<Slot, UnitBase>(81);

            return s_slots;
        }
    }

    LayerMask _layerMank;

    bool _isClickPossible;

    void Awake()
    {
        _layerMank = LayerMask.GetMask("Slot");
        _isClickPossible = true;

        Init();
    }

    void Start()
    {
        UIManager.Instance.Get<UnitBuyUI>().OnBuyButtonClick += () => IsVacancy();
        UIManager.Instance.OnUIChange += value => _isClickPossible = value;

        UnitFactory.Instance.OnUnitCreat += unit =>
        {
            unit.OnDisable += () =>
            {
                Slots[unit.Slot] = null;
            };
        };
    }

    void Update()
    {
        if (!_isClickPossible)
            return;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _layerMank) &&
                    Slots[hit.collider.GetComponent<Slot>()] != null)
                {
                    UIManager.Instance.Get<UnitInfoUI>().Show(hit.collider.GetComponent<Slot>());
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _layerMank) &&
                Slots[hit.collider.GetComponent<Slot>()] != null)
            {
                UIManager.Instance.Get<UnitInfoUI>().Show(hit.collider.GetComponent<Slot>());
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

    void Init()
    {
        Slot[] slots = transform.GetComponentsInChildren<Slot>();
        foreach (Slot slot in slots)
        {
            Slots.Add(slot, null);
        }
    }

    /// <summary>
    /// 유닛이 생성될 공간이 있는지 확인하고 있다면 해당 Slot을 반환해주는 함수 / 빈자리가 없다면 null을 반환
    /// </summary>
    static public Slot IsVacancy()
    {
        foreach (KeyValuePair<Slot, UnitBase> item in Slots)
        {
            if (item.Value == null)
            {
                return item.Key;
            }
        }

        return null;
    }
}