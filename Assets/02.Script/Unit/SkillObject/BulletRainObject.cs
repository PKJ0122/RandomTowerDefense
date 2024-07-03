using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRainObject : SkillObject
{
    const float RANGE = 9f;
    const float TICK_TIME = 0.5f;
    const int LAST_TICK_COUNT = 8;
    const float SLOW_AMOUNT = 0.05f;

    List<Enemy> _slowEnemy = new List<Enemy>(10);
    int _tickCount;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, RANGE);
    }

    public override void ObjectSet(UnitBase caster, Enemy target, float damage)
    {
        base.ObjectSet(caster, target, damage);
        transform.position = _target.transform.position;
        _tickCount = 0;
        StartCoroutine(C_Activity());
    }

    YieldInstruction _delay = new WaitForSeconds(TICK_TIME);

    IEnumerator C_Activity()
    {
        while (_tickCount < LAST_TICK_COUNT)
        {
            ++_tickCount;

            Collider[] targets = Physics.OverlapSphere(transform.position, RANGE, _targetMask);

            foreach (Collider target in targets)
            {
                Enemy enemy = target.GetComponent<Enemy>();
                _target = enemy;
                _target.Speed = SLOW_AMOUNT;
                _slowEnemy.Add(enemy);
                Damage();
            }

            yield return _delay;
        }

        foreach (Enemy ememy in _slowEnemy)
        {
            ememy.Speed = 0.2f;
        }
    }
}
