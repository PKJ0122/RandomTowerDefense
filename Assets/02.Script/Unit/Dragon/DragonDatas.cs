using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DragonDatas", menuName = "ScriptableObject/Dragon/DragonDatas")]
public class DragonDatas : ScriptableObject
{
    public DragonCraftingMethod[] dragonCraftingMethods;
}