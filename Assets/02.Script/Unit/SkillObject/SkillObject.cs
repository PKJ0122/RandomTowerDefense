using System;
using UnityEngine;

public class SkillObject : PoolObject
{
    protected UnitBase _caster;
    protected Enemy _target;
    protected float _damage;
    protected LayerMask _targetMask;

    public event Action OnDisableHandler;

    protected virtual void Awake()
    {
        _targetMask = LayerMask.GetMask("Enemy");

        OnDisableHandler += () => RelasePool();
    }

    protected virtual void OnParticleSystemStopped()
    {
        RelasePool();
    }

    public virtual void ObjectSet(UnitBase caster, Enemy target, float damage)
    {
        _caster = caster;
        _target = target;
        _damage = damage;
        _caster.OnDisable += OnDisableHandler;
    }

    protected virtual void Damage()
    {
        _caster.Damage += _target.Damage(_damage);
    }

    public override void RelasePool()
    {
        _caster.OnDisable -= OnDisableHandler;
        base.RelasePool();
    }
}
