using System;
using UnityEngine.UI;

public class GoldBossUI : UIBase
{
    Button _bossSpawn;
    int _clickWave = -1;

    public event Action OnBossSpawnButtonClick;


    protected override void Awake()
    {
        base.Awake();
        _bossSpawn = transform.Find("Button - GoldBossButton").GetComponent<Button>();
        _bossSpawn.onClick.AddListener(() =>
        {
            OnBossSpawnButtonClick?.Invoke();
            _clickWave = GameManager.Instance.Wave;
            Hide();
        });
    }

    private void Start()
    {
        GameManager.Instance.OnWaveChange += value =>
        {
            if (value == _clickWave + 5) Show();
        };
    }
}