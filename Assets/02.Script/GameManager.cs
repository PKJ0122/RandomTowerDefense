using System;
using System.Collections;
using UnityEngine;

public class GameManager : SingletonMonoBase<GameManager>
{
    const float ROUND_TIME = 60f;
    const int START_GOLD = 100;
    const int INTEREST = 40;

    int _wave = -1;
    public int Wave
    {
        get => _wave;
        set
        {
            _wave = value;
            onWaveChange?.Invoke(value);
        }
    }

    float _time;
    public float TickTime
    {
        get => _time;
        set
        {
            _time = value;
            onTimeChange?.Invoke(value);
        }
    }

    int _gold;
    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            onGoldChange?.Invoke(value);
        }
    }

    public event Action<int> onWaveChange;
    public event Action<float> onTimeChange;
    public event Action<int> onGoldChange;


    protected override void Awake()
    {
        base.Awake();
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif

        onWaveChange += value =>
        {
            if (Wave <= 0) return;
            Gold += INTEREST + (Gold/10);
        };

        StartCoroutine(C_Game());
    }

    YieldInstruction delay = new WaitForSeconds(1f); //�����̷� ����� ��ü

    /// <summary>
    /// ���ӷ��� �ڷ�ƾ
    /// </summary>
    IEnumerator C_Game()
    {
        yield return delay;
        Gold = START_GOLD;
        Wave = 0;

        while (Wave + 1 <= 50)
        {
            TickTime = ROUND_TIME;

            while (TickTime > 0)
            {
                yield return delay;
                TickTime--;
            }

            Wave++;
        }

        GameClear();
    }

    /// <summary>
    /// ���� Ŭ���� �Լ�
    /// </summary>
    void GameClear()
    {

    }

    /// <summary>
    /// ���� ���� �Լ�
    /// </summary>
    public void GameOver()
    {

    }
}
