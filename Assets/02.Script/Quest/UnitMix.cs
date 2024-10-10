using UnityEngine;

[CreateAssetMenu(fileName = "UnitMix", menuName = "ScriptableObject/Quest/UnitMix")]
public class UnitMix : QuestBase
{
    int _count;

    public override void Init()
    {
        UIManager.Instance.Get<UnitInfoUI>().OnUnitMix += v =>
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
