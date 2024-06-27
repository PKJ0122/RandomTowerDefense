using UnityEngine;

public class ShieldThrowingObject : SkillObject
{
    const float ARRIVAL_TIME = 0.3f;

    float _tick;
    bool _isGiveDamege;

    private void Update()
    {
        if (_isGiveDamege)
            return;

        _tick += Time.deltaTime;
        transform.position = Vector3.Lerp(_caster.transform.position, _target.transform.position, _tick / ARRIVAL_TIME);
        transform.LookAt(_target.transform.position);

        if (_tick >= ARRIVAL_TIME)
        {
            Damage();
            _isGiveDamege = true;
            RelasePool();
        }
    }

    public override void ObjectSet(UnitBase caster, Enemy target, float damage)
    {
        base.ObjectSet(caster, target, damage);
        _tick = 0;
        _isGiveDamege = false;
    }
}