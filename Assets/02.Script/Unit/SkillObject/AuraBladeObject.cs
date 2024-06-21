using UnityEngine;

public class AuraBladeObject : SkillObject
{
    public override void ObjectSet(UnitBase caster, Enemy target, float damage)
    {
        base.ObjectSet(caster, target, damage);
        transform.position = Vector3.Lerp(_caster.transform.position, _target.transform.position, 0.1f);
        transform.LookAt(_target.transform.position);
        Damage();
    }
}