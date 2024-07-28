using UnityEngine;

[CreateAssetMenu(fileName = "Wave30OverClear", menuName = "ScriptableObject/Quest/Wave30OverClear")]
public class Wave30OverClear : QuestBase
{
    public override void Init()
    {
        GameManager.Instance.OnWaveChange += value =>
        {
            if (value == 32)
            {
                PlayerData.Instance.SetQuestSaveData(questName, 1);
            }
        };
    }
}