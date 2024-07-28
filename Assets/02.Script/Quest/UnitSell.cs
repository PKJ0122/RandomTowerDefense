using UnityEngine;

[CreateAssetMenu(fileName = "UnitSell", menuName = "ScriptableObject/Quest/UnitSell")]
public class UnitSell : QuestBase
{
    public override void Init()
    {
        UIManager.Instance.Get<UnitInfoUI>().OnUnitSell += v =>
        {
            PlayerData.Instance.SetQuestSaveData(questName, 1);
        };
    }
}