using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AdShow", menuName = "ScriptableObject/Quest/AdShow")]
public class AdShow : QuestBase
{
    public override void Init()
    {
        AdManager.Instance.RewardedAd.OnAdClosed += (object sender, EventArgs args) =>
        {
            PlayerData.Instance.SetQuestSaveData(questName, 1);
        };
    }
}