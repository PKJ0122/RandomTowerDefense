using UnityEngine;

[CreateAssetMenu(fileName = "UnitBeyond", menuName = "ScriptableObject/Quest/UnitBeyond")]
public class UnitBeyond : QuestBase
{
    int _count;

    public override void Init()
    {
        UIManager.Instance.Get<BeyondCraftingUI>().OnBeyond += () =>
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