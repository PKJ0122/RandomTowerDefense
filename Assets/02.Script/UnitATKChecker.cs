using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class UnitATKChecker : SingletonMonoBase<UnitATKChecker>
{
    Dictionary<UnitKind, Dictionary<UnitRank, List<UnitBase>>> units = new Dictionary<UnitKind, Dictionary<UnitRank, List<UnitBase>>>();
    Dictionary<UnitKind, int> unitLevel = new Dictionary<UnitKind, int>();

    BeyondCraftingMethods _beyondCraftingMethods;
    Dictionary<UnitKind, BeyondCraftingCounter> counters = new Dictionary<UnitKind, BeyondCraftingCounter>();

    int[] _unitDPS = new int[] { 10, 35, 150, 650, 3000, 9000 };

    int unitDPS;
    int unitBuyATK = 50;
    int unitBuyCount;
    int gold = 100;
    public int Gold
    {
        get => gold;
        set
        {
            if (gold > value)
            {
                OnGoldChange?.Invoke(gold - value);
            }

            gold = value;
        }
    }

    UnitBuyUI unitBuyUI;
    IObjectPool<PoolObject> pool;

    public event Action<UnitBase> OnUnit;
    public event Action<int> OnGoldChange;
    int zgold;


    private void Start()
    {
        unitBuyUI = UIManager.Instance.Get<UnitBuyUI>();
        _beyondCraftingMethods = Resources.Load<BeyondCraftingMethods>("BeyondCraftingMethods");

        foreach (BeyondCraftingMethod item in _beyondCraftingMethods.beyondCraftingMethods)
        {
            BeyondCraftingCounter counter = new BeyondCraftingCounter(item, this);
            counters.Add(item.unitKind, counter);
        }

        OnUnit += unit =>
        {
            unit.OnDisable += () =>
            {
                units[unit.Kind][unit.Rank].Remove(unit);
                SlotManager.Slots[unit.Slot] = null;
            };
        };

        for (int i = 0; i <= 51; i++)
        {
            bestwaves.Add(i, 0);
            worstwaves.Add(i, int.MaxValue);
        }

        StartCoroutine(C_UnitATCheck());
    }

    Dictionary<int, int> bestwaves = new Dictionary<int, int>();
    Dictionary<int, int> worstwaves = new Dictionary<int, int>();

    YieldInstruction _delay = new WaitForSeconds(0.1f);

    IEnumerator C_UnitATCheck()
    {
        int counting = 0;

        while (counting <= 50)
        {
            unitBuyCount = 0;
            counting++;

            int wave = 0;
            Gold = 100;

            UnitBuy();
            UnitBuy();
            UnitBuy();
            UnitBuy();

            while (wave <= 50)
            {
                wave++;
                Gold += 140;

                if (wave == 15) Gold += 700;

                if (wave % 5 == 0)
                {
                    UnitKind unitKind = (UnitKind)Random.Range(0, Enum.GetValues(typeof(UnitKind)).Length);
                    MixUnitPuls(unitKind, CoinUnit());
                }
                if (wave % 10 == 0)
                {
                    UnitKind unitKind = (UnitKind)Random.Range(0, Enum.GetValues(typeof(UnitKind)).Length);
                    MixUnitPuls(unitKind, CoinUnit());
                }

                Beyoud();
                UnitMix();

                yield return _delay;


                while (Gold >= UnitBuyGold)
                {
                    UnitBuy();
                }

                int nowDPS = 0;

                foreach (var item in units)
                {
                    foreach (var item1 in item.Value)
                    {
                        nowDPS += item1.Value.Count * _unitDPS[(int)item1.Key];
                    }
                }

                bestwaves[wave] = Math.Max(bestwaves[wave], nowDPS);
                worstwaves[wave] = Math.Min(worstwaves[wave], nowDPS);

                yield return _delay;
            }

            foreach (var item in units)
            {
                foreach (var item1 in item.Value)
                {
                    for (int i = item1.Value.Count - 1; i >= 0; i--)
                    {
                        item1.Value[i].RelasePool();
                    }
                }
            }
        }

        HpFunDatas hpFunDatas = new HpFunDatas();

        for (int i = 0; i <= 51; i++)
        {
            float lerpAmout = 0;

            if (i < 10)
            {
                lerpAmout = i == 0 ? 1 / 65 : (float)i / 65;
            }
            else if (i < 20)
            {
                lerpAmout = (float)i / 55;
            }
            else if (i < 30)
            {
                lerpAmout = (float)i / 50;
            }
            else if (i < 40)
            {
                lerpAmout = (float)i / 45;
            }
            else
            {
                lerpAmout = (float)i / 38;
            }

            Debug.Log($"{i} Wave : (Best : {bestwaves[i]}) , (Worst : {worstwaves[i]}) ," +
                $" (Lerp / {lerpAmout} : {Lerp(worstwaves[i], bestwaves[i], lerpAmout)}");

            hpFunDatas.datas[i] = new HpFunData(i, bestwaves[i], worstwaves[i],
                lerpAmout, Lerp(worstwaves[i], bestwaves[i], lerpAmout));
        }

        string filePath = Path.Combine(Application.persistentDataPath, "hpFunDatas.json");

        string json = JsonUtility.ToJson(hpFunDatas, true); // JSON으로 직렬화
        File.WriteAllText(filePath, json); // 파일에 저장
    }

    int Lerp(int start, int end, float magnification)
    {
        return magnification > 1 ? (int)Mathf.Lerp(start, end, magnification) + (int)((magnification - 1) * end)
            : (int)Mathf.Lerp(start, end, magnification);
    }
    void UnitBuy()
    {
        UnitBase unit = RandomUnit();
        Gold -= UnitBuyGold;
        unitBuyCount++;
        this.units[unit.Kind][unit.Rank].Add(unit);
        OnUnit?.Invoke(unit);
    }

    int UnitBuyGold => 20 + (unitBuyCount * 1);

    [Serializable]
    class HpFunDatas
    {
        public HpFunData[] datas;

        public HpFunDatas()
        {
            datas = new HpFunData[52];
        }
    }

    [Serializable]
    class HpFunData
    {
        public int wave;
        public int bestDPS;
        public int worstDPS;
        public float lerpPer;
        public int lerpDPS;
        public float enemyHp;

        public HpFunData(int wave, int bestDPS, int worstDPS, float lerpPer, int lerpDPS)
        {
            this.wave = wave;
            this.bestDPS = bestDPS;
            this.worstDPS = worstDPS;
            this.lerpPer = lerpPer;
            this.lerpDPS = lerpDPS;
            enemyHp = (float)lerpDPS * 60 / 40;
        }
    }

    UnitRank CoinUnit()
    {
        int RandomNum = Random.Range(0, 100);

        if (0 <= RandomNum && RandomNum < 10)
        {
            return UnitRank.Legendary;
        }
        else if (10 <= RandomNum && RandomNum < 30)
        {
            return UnitRank.Unique;
        }
        else
        {
            return UnitRank.Epic;
        }
    }

    void DebugWrite(int wave)
    {
        foreach (var item in units)
        {
            foreach (var item1 in item.Value)
            {
                unitDPS += unitLevel.ContainsKey(item.Key) ?
                    (_unitDPS[(int)item1.Key] + (_unitDPS[(int)item1.Key] / 10 * unitLevel[item.Key])) * item1.Value.Count
                    : _unitDPS[(int)item1.Key] * item1.Value.Count;
            }
        }

        Debug.Log($"{wave} wave : {unitDPS}");

        unitDPS = 0;
    }

    void Beyoud()
    {
        foreach (var item in counters)
        {
            if (item.Value.IsBeyondCraftingPossible)
            {
                foreach (var item1 in item.Value.Materials)
                {
                    item1[0].RelasePool();
                }

                UnitKind unitKind = item.Key;
                MixUnitPuls(unitKind, UnitRank.Beyond);
            }
        }
    }

    bool Best()
    {
        Debug.Log("zz");

        Dictionary<UnitKind, int> pulsAtk = new Dictionary<UnitKind, int>();
        foreach (var item in units)
        {
            foreach (var item1 in item.Value)
            {
                int num = (int)item1.Key;

                if (!pulsAtk.ContainsKey(item.Key))
                {
                    pulsAtk.Add(item.Key, 0);
                }
                pulsAtk[item.Key] += item1.Value.Count * _unitDPS[num] / 10;
            }
        }

        (UnitKind unitKind, int gold, int PuslDps) best = (UnitKind.Spike, 0, 0);

        foreach (var item in pulsAtk)
        {
            if (best.gold == 0)
            {
                best.unitKind = item.Key;
                best.gold = unitLevel.ContainsKey(item.Key) ? 100 + (unitLevel[item.Key]) * 50 : 100;
                best.PuslDps = best.gold / item.Value;
                continue;
            }

            int num = unitLevel.ContainsKey(item.Key) ? 100 + (unitLevel[item.Key]) * 50 : 100 / item.Value;

            Debug.Log(num);

            if (best.PuslDps < num)
            {
                best.unitKind = item.Key;
                best.gold = unitLevel.ContainsKey(item.Key) ? 100 + (unitLevel[item.Key]) * 50 : 100;
                best.PuslDps = num;
            }
        }

        int unitBuycount = best.gold / UnitBuyGold;
        int unitBuyDps = unitBuycount * unitBuyATK;

        Debug.Log(unitBuyCount);

        if (unitBuyDps >= best.PuslDps * best.gold)
        {
            if (gold < UnitBuyGold) return false;

            UnitBuy();
        }
        else
        {
            if (gold < best.gold) return false;

            UnitLevelUp(best.unitKind, best.gold);
        }

        return true;
    }


    UnitBase RandomUnit()
    {
        UnitKind unitKind = (UnitKind)Random.Range(0, Enum.GetValues(typeof(UnitKind)).Length);
        UnitRank unitRank = unitBuyUI.RandomRank();

        if (!this.units.ContainsKey(unitKind))
        {
            this.units.Add(unitKind, new Dictionary<UnitRank, List<UnitBase>>());
        }

        if (!this.units[unitKind].ContainsKey(unitRank))
        {
            this.units[unitKind].Add(unitRank, new List<UnitBase>());
        }

        UnitBase unit = ObjectPoolManager.Instance.Get($"{unitKind}").Get().GetComponent<UnitBase>();

        unit.UnitSet(SlotManager.IsVacancy(), unitKind, unitRank);

        return unit;
    }

    void MixUnitPuls(UnitKind unitKind, UnitRank unitRank)
    {
        if (!this.units.ContainsKey(unitKind))
        {
            this.units.Add(unitKind, new Dictionary<UnitRank, List<UnitBase>>());
        }

        if (!this.units[unitKind].ContainsKey(unitRank))
        {
            this.units[unitKind].Add(unitRank, new List<UnitBase>());
        }

        UnitBase unit = ObjectPoolManager.Instance.Get($"{unitKind}").Get().GetComponent<UnitBase>();
        unit.UnitSet(SlotManager.IsVacancy(), unitKind, unitRank);

        this.units[unitKind][unitRank].Add(unit);
        OnUnit?.Invoke(unit);
    }

    struct Zz
    {
        public UnitKind kind;
        public UnitRank rank;
        public int count;

        public Zz(UnitKind kind, UnitRank rank, int count)
        {
            this.kind = kind;
            this.rank = rank;
            this.count = count;
        }
    }

    void UnitMix()
    {
        List<Zz> possible = new List<Zz>();

        foreach (var item in units)
        {
            foreach (var item1 in item.Value)
            {
                if (item1.Key == UnitRank.Legendary || item1.Key == UnitRank.Beyond) continue;

                if (item1.Value.Count <= 2) continue;

                bool isM = false;

                if (units[item.Key].ContainsKey(UnitRank.Legendary))
                {
                    if (units[item.Key][UnitRank.Legendary].Count >= 1)
                    {
                        foreach (var item2 in counters[item.Key].Method.beyondCraftingMaterials)
                        {
                            if (item2.unitKind == item.Key && item2.unitRank == item1.Key)
                            {
                                isM = true;
                                break;
                            }
                        }
                    }
                }

                int num = isM ? item1.Value.Count - 1 : item1.Value.Count;

                int conut = num / 3;

                Zz zz = new Zz(item.Key, item1.Key, conut);

                possible.Add(zz);
            }
        }

        foreach (var item in possible)
        {
            List<UnitBase> unitz = units[item.kind][item.rank];

            for (int j = 0; j < item.count; j++)
            {
                for (int i = unitz.Count - 1; i >= 0 && i >= unitz.Count - 3; i--)
                {
                    unitz[i].RelasePool();
                }
                UnitKind unitKind = (UnitKind)Random.Range(0, Enum.GetValues(typeof(UnitKind)).Length);
                MixUnitPuls(unitKind, (UnitRank)((int)item.rank + 1));
            }
        }
    }

    void UnitLevelUp(UnitKind unitKind, int gold)
    {
        if (!unitLevel.ContainsKey(unitKind))
        {
            unitLevel.Add(unitKind, 0);
        }

        this.gold -= gold;
        unitLevel[unitKind]++;
    }
}
