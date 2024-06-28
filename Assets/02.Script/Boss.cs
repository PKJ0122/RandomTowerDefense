using System;
using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    private void OnEnable()
    {
        StartCoroutine(C_BossActivity());
    }

    YieldInstruction delay = new WaitForSeconds(60.5f);

    IEnumerator C_BossActivity()
    {
        yield return delay;

        //todo -> 게임오버
    }

    protected override void Die()
    {
        GameManager.Instance.Gold += 99;
        base.Die();
    }
}