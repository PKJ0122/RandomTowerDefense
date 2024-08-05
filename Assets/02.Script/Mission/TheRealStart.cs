using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TheRealStart", menuName = "ScriptableObject/Mission/TheRealStart")]
public class TheRealStart : MissionBase
{
    bool _isGameIng = true;

    public override void Init()
    {
        base.Init();
        GameManager.Instance.OnGameStart += () => _isGameIng = true;
        GameManager.Instance.OnGameEnd += v => _isGameIng = false;
        GameManager.Instance.OnEnemyAmountChange += value =>
        {
            if (!_isGameIng) return;

            Progress = value;
        };
    }
}
