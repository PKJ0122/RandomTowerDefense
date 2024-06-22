using UnityEngine;

public class SkillObject : PoolObject
{
    protected UnitBase _caster;
    protected Enemy _target;
    protected float _damage;
    protected LayerMask _targetMask;

    void Awake()
    {
        _targetMask = LayerMask.GetMask("Enemy");
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
    }

    protected virtual void Damage()
    {
        _caster.Damage += _target.Damage(_damage);
    }
}
