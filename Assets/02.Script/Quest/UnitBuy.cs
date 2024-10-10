using UnityEngine;

[CreateAssetMenu(fileName = "UnitBuy", menuName = "ScriptableObject/Quest/UnitBuy")]
public class UnitBuy : QuestBase
{
    int _count;

    public override void Init()
    {
        UIManager.Instance.Get<UnitBuyUI>().OnUnitBuy += () =>
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