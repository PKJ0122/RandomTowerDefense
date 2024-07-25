using System;

[Serializable]
public class QuestSaveData
{
    public string QuestName;
    int _amount;
    public int Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            OnAmountChange?.Invoke(value);
        }
    }

    public event Action<int> OnAmountChange;


    public void Reset()
    {
        OnAmountChange = null;
    }
}