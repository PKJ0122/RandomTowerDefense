using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBase<GameManager>
{
    Dictionary<UnitKind, List<Slot>> _units = new Dictionary<UnitKind, List<Slot>>();
    public Dictionary<UnitKind, List<Slot>> Units { get => _units; }

    const float ROUND_TIME = 60f;
    const int START_GOLD = 10000;
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

    int _enemyAmount;
    public int EnemyAmount
    {
        get => _enemyAmount;
        set
        {
            _enemyAmount = value;
            onEnemyAmountChange?.Invoke(value);
        }
    }

    public event Action<int> onWaveChange;
    public event Action<float> onTimeChange;
    public event Action<int> onGoldChange;
    public event Action<int> onEnemyAmountChange;


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

    private void Start()
    {
        UIManager.Instance.Get<UnitBuyUI>().onUnitBuySuccess += (slot, unit) => RegisterUnit(slot, unit);
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
    /// ������ ���� ��ųʸ��� ����ϴ� �Լ�
    /// </summary>
    /// <param name="unit"></param>
    void RegisterUnit(Slot slot,UnitBase unit)
    {
        if (!Units.TryGetValue(unit.Kind,out List<Slot> list))
        {
            list = new List<Slot>();
            Units.Add(unit.Kind, list);
        }
        list.Add(slot);
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
