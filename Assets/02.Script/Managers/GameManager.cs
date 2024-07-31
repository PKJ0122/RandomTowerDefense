using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : SingletonMonoBase<GameManager>
{
    const float ROUND_TIME = 60f;
    const int START_GOLD = 100;
    const int SALARY = 40;
    const int INTEREST = 10;

    Dictionary<UnitKind, List<UnitBase>> _units = new Dictionary<UnitKind, List<UnitBase>>(8);
    public Dictionary<UnitKind, List<UnitBase>> Units { get => _units; }

    public int Salary { get; set; } = SALARY;
    public int Interest { get; set; } = INTEREST;

    int _wave = -1;
    public int Wave
    {
        get => _wave;
        set
        {
            _wave = value;
            OnWaveChange?.Invoke(value);
        }
    }

    float _time;
    public float TickTime
    {
        get => _time;
        set
        {
            _time = value;
            OnTimeChange?.Invoke(value);
        }
    }

    int _gold;
    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            OnGoldChange?.Invoke(value);
        }
    }

    int _enemyAmount;
    public int EnemyAmount
    {
        get => _enemyAmount;
        set
        {
            _enemyAmount = value;
            OnEnemyAmountChange?.Invoke(value);
        }
    }

    public event Action<int> OnWaveChange;
    public event Action<float> OnTimeChange;
    public event Action<int> OnGoldChange;
    public event Action<int> OnEnemyAmountChange;
    public event Action OnGameStart;
    public event Action<int> OnGameEnd;


    protected override void Awake()
    {
        base.Awake();

        OnWaveChange += value =>
        {
            if (Wave <= 0) return;
            Gold += Salary + (Gold/Interest);
        };

        StartCoroutine(C_Game());
    }

    private void Start()
    {
        UIManager.Instance.Get<UnitBuyUI>().OnUnitBuySuccess += (unit) => RegisterUnit(unit);
        UIManager.Instance.Get<GameEndUI>().OnReStartButtonClick += GameReStart;
    }

    YieldInstruction delay = new WaitForSeconds(1f); //딜레이로 사용할 객체

    /// <summary>
    /// 게임로직 코루틴
    /// </summary>
    IEnumerator C_Game()
    {
        yield return delay;
        OnGameStart?.Invoke();
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

        GameEnd();
    }

    /// <summary>
    /// 유닛을 유닛 딕셔너리에 등록하는 함수
    /// </summary>
    /// <param name="unit"></param>
    void RegisterUnit(UnitBase unit)
    {
        if (!Units.TryGetValue(unit.Kind,out List<UnitBase> list))
        {
            list = new List<UnitBase>();
            Units.Add(unit.Kind, list);
        }
        list.Add(unit);
    }

    /// <summary>
    /// 게임 종료 함수
    /// </summary>
    public void GameEnd()
    {
        OnGameEnd?.Invoke(Wave);
        UIManager.Instance.Get<GameEndUI>().Show(Wave);
    }

    void GameReStart()
    {
        foreach (KeyValuePair<UnitKind, List<UnitBase>> item in Units)
        {
            for (int i = item.Value.Count - 1; i >= 0; i--)
            {
                item.Value[i].RelasePool();
            }
        }
        StopAllCoroutines();
        StartCoroutine(C_Game());
    }
}
