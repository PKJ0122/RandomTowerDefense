using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundBoss : Boss
{
    protected override IEnumerator C_BossActivity()
    {
        yield return _delay;
        GameManager.Instance.GameEnd();
    }
}
