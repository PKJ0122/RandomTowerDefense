using UnityEngine;

[CreateAssetMenu(fileName = "BeyondCraftingMethod", menuName = "ScriptableObject/Beyond/BeyondCraftingMethod")]
public class BeyondCraftingMethod : ScriptableObject
{
    public UnitKind unitKind;
    public UnitInfo[] beyondCraftingMaterials;
}