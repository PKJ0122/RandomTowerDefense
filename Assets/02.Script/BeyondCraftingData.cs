using System;
using UnityEngine;

[Serializable]
public class BeyondCraftingData
{
    public UnitKind unitKind;
    [SerializeField] bool _isHave;
    public bool IsHave
    {
        get => _isHave;
        set
        {
            _isHave = value;
            OnIsHaveChange?.Invoke(value);
        }
    }

    public event Action<bool> OnIsHaveChange;
}