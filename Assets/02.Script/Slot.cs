using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    void Awake()
    {
        if (SlotManager.Slots.TryAdd(this, null))
        {
            Debug.LogWarning("�̹� ��ϵ� ���� �Դϴ�.");
        }
    }
}
