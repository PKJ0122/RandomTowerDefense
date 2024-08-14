using System;
using UnityEngine;

[Serializable]
public class ItemLevelData
{
    public string itemName;
    public int level;
    [SerializeField] int _amount;

    public int Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            if (level == 0 && _amount >= 1)
            {
                _amount--;
                level++;
            }
        }
    }
}