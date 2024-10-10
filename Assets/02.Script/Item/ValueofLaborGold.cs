using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ValueofLaborGold", menuName = "ScriptableObject/Item/ValueofLaborGold")]
public class ValueofLaborGold : ItemBase
{
    protected override void Use()
    {
        GameManager.Instance.OnGameEnd += value =>
        {
            if (value < 40) return;

            PlayerData.Instance.Gold += (int)Value;
        };
    }
}