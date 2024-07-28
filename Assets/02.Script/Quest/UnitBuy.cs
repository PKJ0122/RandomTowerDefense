using UnityEngine;

[CreateAssetMenu(fileName = "UnitBuy", menuName = "ScriptableObject/Quest/UnitBuy")]
public class UnitBuy : QuestBase
{
    public override void Init()
    {
        UIManager.Instance.Get<UnitBuyUI>().OnUnitBuySuccess += v =>
        {
            PlayerData.Instance.SetQuestSaveData(questName, 1);
        };
    }
}