using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeyondCraftingMethods", menuName = "ScriptableObject/BeyondCraftingMethods")]
[Serializable]
public class BeyondCraftingMethods : ScriptableObject
{
    public BeyondCraftingMethod[] beyondCraftingMethods;

    Dictionary<UnitKind, BeyondCraftingMethod> _beyondCraftingMethodDatas;
    public Dictionary<UnitKind, BeyondCraftingMethod> BeyondCraftingMethodDatas
    {
        get
        {
            if (_beyondCraftingMethodDatas == null)
            {
                _beyondCraftingMethodDatas = new Dictionary<UnitKind, BeyondCraftingMethod>();
                foreach (BeyondCraftingMethod item in beyondCraftingMethods)
                {
                    _beyondCraftingMethodDatas.Add(item.unitKind, item);
                }
            }
            return _beyondCraftingMethodDatas;
        }
    }
}
