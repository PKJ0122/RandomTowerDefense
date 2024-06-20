using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillObject : PoolObject
{
    UnitBase _caster;
    Enemy _target;
    float _damage;


    public virtual void ObjectSet(UnitBase caster,Enemy target,float damage)
    {
        _caster = caster;
        _target = target;
        _damage = damage;
    }
}
