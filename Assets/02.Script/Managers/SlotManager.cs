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

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _layerMank))
                {
                    
                }
            }
        }
    }
}
