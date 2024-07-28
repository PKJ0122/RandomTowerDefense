using UnityEngine;

[CreateAssetMenu(fileName = "UnitLevelUp", menuName = "ScriptableObject/Quest/UnitLevelUp")]
public class UnitLevelUp : QuestBase
{
    public override void Init()
    {
        UIManager.Instance.Get<UnitLevelUpUI>().OnUnitLevelUpSuccess += () =>
        {
            PlayerData.Instance.SetQuestSaveData(questName, 1);
        };
    }
}