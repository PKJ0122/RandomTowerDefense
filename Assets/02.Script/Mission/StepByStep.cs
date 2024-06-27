using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StepByStep", menuName = "ScriptableObject/Mission/StepByStep")]
public class StepByStep : MissionBase
{
    public override void Init()
    {
        base.Init();
        GameManager.Instance.onGoldChange += value =>
        {
            Progress = value;
        }; ;
    }
}
