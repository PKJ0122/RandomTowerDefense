using UnityEngine;

[CreateAssetMenu(fileName = "UnitSell", menuName = "ScriptableObject/Quest/UnitSell")]
public class UnitSell : QuestBase
{
    int _count;

    public override void Init()
    {
        UIManager.Instance.Get<UnitInfoUI>().OnUnitSell += v =>
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