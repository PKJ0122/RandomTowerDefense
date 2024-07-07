using System;

[Serializable]
public class ItemLevelData
{
    public string itemName;
    public int level;
    private int _amount;

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