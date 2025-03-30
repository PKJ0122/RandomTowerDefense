using System;
using UnityEditor.Build.Pipeline;
using UnityEngine;
using UnityEngine.Localization.Settings;

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
    
    public string ItemName()
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", itemName, LocalizationSettings.SelectedLocale);
    }

    public string Description()
    {
        string de = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", description, LocalizationSettings.SelectedLocale);
        return de.Replace("$", $"<color=red><b>{Value}</color></b>");
    }


    public void TryUse()
    {
        if (!PlayerData.ItemLevels.ContainsKey(itemName)) return;
        Use();
    }

    protected abstract void Use();
}
