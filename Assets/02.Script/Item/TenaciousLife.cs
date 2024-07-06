using UnityEngine;

[CreateAssetMenu(fileName = "TenaciousLife", menuName = "ScriptableObject/Item/TenaciousLife")]
public class TenaciousLife : ItemBase
{
    protected override void Use()
    {
        UIManager.Instance.Get<EnemyAmountUI>().DieAmount += (int)Value;
        UIManager.Instance.Get<ItemUseEffectUI>().Show(itemImage, itemName);
    }
}
