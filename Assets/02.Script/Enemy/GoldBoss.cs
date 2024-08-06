using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBoss : Boss
{
    protected override IEnumerator C_BossActivity()
    {
        yield return _delay;
        RelasePool();
    }
}
