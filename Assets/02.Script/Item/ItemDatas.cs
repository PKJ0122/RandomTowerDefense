using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatas", menuName = "ScriptableObject/ItemDatas")]
[Serializable]
public class ItemDatas : ScriptableObject
{
    public ItemBase[] itemDatas;
}
