using UnityEngine;

[CreateAssetMenu(fileName = "ValueofLaborDiamond", menuName = "ScriptableObject/Item/ValueofLaborDiamond")]
public class ValueofLaborDiamond : ItemBase
{
    protected override void Use()
    {
        GameManager.Instance.OnGameEnd += value =>
        {
            if (value < 40) return;

            PlayerData.Instance.Diamond += (int)Value;
            UIManager.Instance.Get<ItemUseEffectUI>().Show(itemImage, itemName);
        };
    }
}