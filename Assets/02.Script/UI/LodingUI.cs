using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LodingUI : UIBase
{
    public override void Show()
    {
        base.Show();
        SortingOrder = 999999;
    }
}
