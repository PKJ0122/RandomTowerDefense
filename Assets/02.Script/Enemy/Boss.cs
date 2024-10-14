using System;
using System.Collections;
using UnityEngine;

public abstract class Boss : Enemy
{
    public override float Hp
    {
        get => base.Hp;
        set
        {
            base.Hp = value;
            OnHpChange?.Invoke(value);
        }
    }

    public event Action<float> OnHpChange;
    public event Action OnDie;


    protected override void Awake()
    {
        Priority++;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(C_BossActivity());
    }

    protected YieldInstruction _delay = new WaitForSeconds(60.5f);

    protected abstract IEnumerator C_BossActivity();

    protected override void Die()
    {
        GameManager.Instance.Key++;
        OnDie?.Invoke();
        OnDie = null;
        base.Die();
    }
}