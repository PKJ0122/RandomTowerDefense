using UnityEngine;

[CreateAssetMenu(fileName = "HighInterest", menuName = "ScriptableObject/Item/HighInterest")]
public class HighInterest : ItemBase
{
    protected override void Use()
    {
        GameManager.Instance.Gold += (int)Value;
        UIManager.Instance.Get<ItemUseEffectUI>().Show(itemImage, itemName);
    }
}
