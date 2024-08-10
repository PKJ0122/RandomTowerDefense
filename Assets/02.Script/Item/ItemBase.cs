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

    public int Level
    {
        get
        {
            if (!PlayerData.ItemLevels.ContainsKey(itemName)) return 0;
            
            return PlayerData.ItemLevels[itemName].level;
        }
    }

    public float Value => (early + Mathf.Max(0,Level - 1) * weight);
    public string Description => description.Replace("Amount", $"<color=red><b>{Value}</color></b>");

    public event Action<ItemBase> OnItemUse;


    public void TryUse()
    {
        if (!PlayerData.ItemLevels.ContainsKey(itemName)) return;
        Use();
    }

    protected abstract void Use();

    protected void Notice()
    {
        OnItemUse?.Invoke(this);
    }
}
