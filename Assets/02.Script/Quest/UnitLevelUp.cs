using UnityEngine;

[CreateAssetMenu(fileName = "UnitLevelUp", menuName = "ScriptableObject/Quest/UnitLevelUp")]
public class UnitLevelUp : QuestBase
{
    int _count;

    public override void Init()
    {
        UIManager.Instance.Get<UnitLevelUpUI>().OnUnitLevelUpSuccess += () =>
        {
            _count++;
        };

        GameManager.Instance.OnGameEnd += v =>
        {
            PlayerData.Instance.SetQuestSaveData(questName, _count);
            _count = 0;
        };
    }
}