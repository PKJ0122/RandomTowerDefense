using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public UnitBase unitBase;

    void Awake()
    {
        if (!SlotManager.Slots.TryAdd(this, unitBase))
        {
            Debug.LogWarning("이미 등록된 슬롯 입니다.");
        }
    }
}
