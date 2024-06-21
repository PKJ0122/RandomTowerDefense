public class StrengthEnhancementObject : SkillObject
{
    public override void ObjectSet(UnitBase caster, Enemy target, float damage)
    {
        base.ObjectSet(caster, target, damage);
        transform.position = caster.transform.position;
        _caster.Power += _caster.Power;
    }

    public override void RelasePool()
    {
        base.RelasePool();
        _caster.Power -= _caster.Power;
    }
}