using UnityEngine;

[CreateAssetMenu(fileName = "UnitMix", menuName = "ScriptableObject/Quest/UnitMix")]
public class UnitMix : QuestBase
{
    public override void Init()
    {
        UIManager.Instance.Get<UnitInfoUI>().OnUnitMix += v =>
        {
            PlayerData.Instance.SetQuestSaveData(questName, 1);
        };
    }
}
