using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeyondCraftingCounterManager : SingletonMonoBase<BeyondCraftingCounterManager>
{
    BeyondCraftingMethods _beyondCraftingMethods;

    public event Action<BeyondCraftingCounter> OnCounterCreate;


    protected override void Awake()
    {
        base.Awake();
        _beyondCraftingMethods = Resources.Load<BeyondCraftingMethods>("BeyondCraftingMethods");
    }

    private void Start()
    {
        foreach (var item in PlayerData.BeyondCraftingDatas)
        {
            if (!item.Value.IsHave) continue; 

            BeyondCraftingCounter counter =
                new BeyondCraftingCounter(_beyondCraftingMethods.BeyondCraftingMethodDatas[item.Key]);

            OnCounterCreate?.Invoke(counter);
        }
    }
}