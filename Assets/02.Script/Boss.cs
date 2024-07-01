using System;
using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    private void Awake()
    {
        Priority++;
    }

    private void OnEnable()
    {
        StartCoroutine(C_BossActivity());
    }

    YieldInstruction delay = new WaitForSeconds(60.5f);

    IEnumerator C_BossActivity()
    {
        yield return delay;
        GameManager.Instance.GameEnd();
    }

    protected override void Die()
    {
        GameManager.Instance.Gold += 99;
        base.Die();
    }
}