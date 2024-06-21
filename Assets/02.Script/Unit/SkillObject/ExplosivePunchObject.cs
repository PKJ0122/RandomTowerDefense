using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivePunchObject : SkillObject
{
    public override void ObjectSet(UnitBase caster, Enemy target, float damage)
    {
        base.ObjectSet(caster, target, damage);
        transform.position = target.transform.position;
    }
}