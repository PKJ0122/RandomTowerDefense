using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SkillNotLuck", menuName = "ScriptableObject/Mission/SkillNotLuck")]
public class SkillNotLuck : MissionBase
{
    public override void Init()
    {
        base.Init();
        UIManager.Instance.Get<UnitBuyUI>().OnUnitBuySuccess += unit =>
        {
            if (unit.Rank == UnitRank.Legendary) Progress++;
        };
    }
}
