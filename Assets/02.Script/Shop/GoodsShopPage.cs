using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsShopPage : ShopPageBase
{
    protected override void Awake()
    {
        base.Awake();
        _toggleObjectName = "Toggle - GoodsShop";
    }

    protected override void ShopRefresh()
    {
        throw new System.NotImplementedException();
    }
}
