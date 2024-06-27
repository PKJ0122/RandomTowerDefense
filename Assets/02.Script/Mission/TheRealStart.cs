using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TheRealStart", menuName = "ScriptableObject/Mission/TheRealStart")]
public class TheRealStart : MissionBase
{
    public override void Init()
    {
        base.Init();
        GameManager.Instance.onEnemyAmountChange += value =>
        {
            Progress = value;
        };
    }
}
