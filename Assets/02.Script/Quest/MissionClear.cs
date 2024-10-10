using UnityEngine;

[CreateAssetMenu(fileName = "MissionClear", menuName = "ScriptableObject/Quest/MissionClear")]
public class MissionClear : QuestBase
{
    int _count;

    public override void Init()
    {
        UIManager.Instance.Get<MissionUI>().OnMissionClear += missonBase =>
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