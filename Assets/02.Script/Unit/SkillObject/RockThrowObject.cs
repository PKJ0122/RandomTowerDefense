using System.Collections;
using UnityEngine;

public class RockThrowObject : SkillObject
{
    const int MAX_ENEMY = 6;

    const float RANGE = 3f;
    const float THROWING_TIME = 0.5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, RANGE);
    }

    public override void ObjectSet(UnitBase caster, Enemy target, float damage)
    {
        base.ObjectSet(caster, target, damage);
        StartCoroutine(C_Activity());
    }

    YieldInstruction _delay = new WaitForSeconds(2f);

    IEnumerator C_Activity()
    {
        float tick = 0;

        Vector3 weight = new Vector3(0, 10f, 0);
        Vector3 middle = Vector3.Lerp(_caster.transform.position, _target.transform.position, 0.5f) + weight;

        while (tick <= THROWING_TIME)
        {
            tick += Time.deltaTime;
            transform.position = BezierCurves(_caster.transform.position, middle, _target.transform.position, tick / THROWING_TIME);
            transform.Rotate(10f, 0, 0);
            yield return null;
        }

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        Collider[] targets = Physics.OverlapSphere(transform.position, RANGE, _targetMask);

        int num = 0;

        foreach (Collider target in targets)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            _target = enemy;
            _target.Speed = 0;
            Damage();

            if (++num >= MAX_ENEMY)
            {
                break;
            }
        }

        yield return _delay;

        foreach (Collider target in targets)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            enemy.Speed = 0.2f;
        }

        meshRenderer.enabled = true;
        RelasePool();
    }

    Vector3 BezierCurves(Vector3 v1, Vector3 v2, Vector3 v3, float t)
    {
        return Vector3.Lerp(Vector3.Lerp(v1, v2, t), Vector3.Lerp(v2, v3, t), t);
    }
}
