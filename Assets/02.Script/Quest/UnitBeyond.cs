using UnityEngine;

[CreateAssetMenu(fileName = "UnitBeyond", menuName = "ScriptableObject/Quest/UnitBeyond")]
public class UnitBeyond : QuestBase
{
    public override void Init()
    {
        UIManager.Instance.Get<BeyondCraftingUI>().OnBeyond += () =>
        {
            PlayerData.Instance.SetQuestSaveData(questName, 1);
        };
    }
}