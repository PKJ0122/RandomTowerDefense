using UnityEngine;

[CreateAssetMenu(fileName = "TheFirstStep", menuName = "ScriptableObject/Mission/TheFirstStep")]
public class TheFirstStep : MissionBase
{
    public override void Init()
    {
        base.Init();
        UIManager.Instance.Get<GoldBossUI>().OnBossSpawnButtonClick += () =>
        {
            if (GameManager.Instance.Wave < 9) Progress++;
        };
    }
}