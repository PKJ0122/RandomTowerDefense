using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AceShotObject : SkillObject
{
    const float ARRIVAL_TIME = 1f;

    float _tick;
    bool _isGiveDamege;

    private void Update()
    {
        if (_isGiveDamege)
            return;

        if (!_target.gameObject.activeSelf)
        {
            transform.position = Vector3.zero;
        }

        _tick += Time.deltaTime;
        transform.position = Vector3.Lerp(_caster.transform.position, _target.transform.position, _tick);

        if (_tick >= ARRIVAL_TIME)
        {
            Damage();
            _isGiveDamege = true;
        }
    }

    public override void ObjectSet(UnitBase caster, Enemy target, float damage)
    {
        base.ObjectSet(caster, target, damage);
        _tick = 0;
        _isGiveDamege = false;
    }
}
