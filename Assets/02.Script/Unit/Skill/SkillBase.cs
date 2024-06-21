using System;
using UnityEngine;

[Serializable]
public abstract class SkillBase : ScriptableObject
{
    public string skillname;
    [Multiline(5)]
    public string description;
    public Sprite skillImg;
    public int needMp;
    public float coefficient;

    public PoolObject effetObject;

    public virtual void Skill(UnitBase caster, Enemy target,float power)
    {
        ObjectPoolManager.Instance.Get(skillname)
                                  .Get()
                                  .GetComponent<SkillObject>()
                                  .ObjectSet(caster, target, power * coefficient);
    }

    public virtual void PoolSet()
    {
        if (effetObject == null)
            return;

        ObjectPoolManager.Instance.CreatePool(skillname, effetObject);
    }
}