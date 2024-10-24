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
            _bossSpawn.gameObject.SetActive(false);
        });
        _bossSpawn.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        GameManager.Instance.OnGameEnd += v =>
        {
            _clickWave = -1;
            _bossSpawn.gameObject.SetActive(false);
        };
    }

    private void Start()
    {
        GameManager.Instance.OnWaveChange += value =>
        {
            if (value == _clickWave + 5) _bossSpawn.gameObject.SetActive(true);
        };
    }
}