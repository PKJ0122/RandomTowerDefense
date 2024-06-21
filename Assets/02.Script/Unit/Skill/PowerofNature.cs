using UnityEngine;

[CreateAssetMenu(fileName = "PowerofNature", menuName = "ScriptableObject/Skill/PowerofNature")]

public class PowerofNature : SkillBase
{
    const int Skill_AMOUNT = 50;

    public override void Skill(UnitBase caster, Enemy target, float power)
    {
        GameManager.Instance.Gold += Skill_AMOUNT;
    }
}
