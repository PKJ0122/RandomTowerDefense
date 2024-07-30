using UnityEngine;

[CreateAssetMenu(fileName = "DragonCraftingMethod", menuName = "ScriptableObject/Dragon/DragonCraftingMethod")]
public class DragonCraftingMethod : ScriptableObject
{
    public DragonBase dragonObject;
    public string dragonName;
    public UnitKind unitKind;
    public DragonCraftingMaterials[] dragonCraftingMaterials;
}