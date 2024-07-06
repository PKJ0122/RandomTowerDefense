using System;
using UnityEngine;

[Serializable]
public abstract class ItemBase : ScriptableObject
{
    public string itemName;
    [Multiline(5)]
    public string description;
    public Sprite itemImage;

    public float early;
    public float weight;

    int _level;
    public int Level
    {
        get => _level;
        set
        {
            _level = value;
            OnLevelChange?.Invoke(value);
        }
    }
    int _amount;
    public int Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            if (Level == 0 && _amount >= 1)
            {
                Level++;
                _amount--;
            }
            OnAmountChange?.Invoke(value);
        }
    }
    public float Value => (early + (Level * weight));

    public event Action<int> OnLevelChange;
    public event Action<int> OnAmountChange;


    public void TryUse()
    {
        if (_level == 0) return;
        Use();
    }

    protected abstract void Use();
}
