using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    void Awake()
    {
        if (SlotManager.Slots.TryAdd(this, null))
        {
            Debug.LogWarning("이미 등록된 슬롯 입니다.");
        }
    }
}
