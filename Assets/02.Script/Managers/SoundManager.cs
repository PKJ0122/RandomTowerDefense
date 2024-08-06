using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBase<SoundManager>
{
    protected override void Awake()
    {
        base.Awake();
        UnitFactory.Instance.OnUnitCreat += unit =>
        {
            if ((int)unit.Rank >= (int)UnitRank.Unique) Vibrate();
        };
    }

    public void Vibrate()
    {
        Handheld.Vibrate();
    }
}
