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
        get => PlayerData.itemLevels[itemName].level;
    }

    public float Value => (early + (Level * weight));

    public void TryUse()
    {
        if (!PlayerData.itemLevels.ContainsKey(itemName)) return;
        Use();
    }

    protected abstract void Use();
}
