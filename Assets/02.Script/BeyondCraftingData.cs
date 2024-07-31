using System;

public class BeyondCraftingData
{
    public UnitKind unitKind;
    bool _isHave;
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