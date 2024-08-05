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

    YieldInstruction _delay = new WaitForSeconds(60.5f);

    IEnumerator C_BossActivity()
    {
        yield return _delay;
        GameManager.Instance.GameEnd();
    }

    protected override void Die()
    {
        GameManager.Instance.Key++;
        base.Die();
    }
}