using UnityEngine;

[CreateAssetMenu(fileName = "MissionClear", menuName = "ScriptableObject/Quest/MissionClear")]
public class MissionClear : QuestBase
{
    public override void Init()
    {
        UIManager.Instance.Get<MissionUI>().OnMissionClear += missonBase =>
        {
            PlayerData.Instance.SetQuestSaveData(questName, 1);
        };
    }
}