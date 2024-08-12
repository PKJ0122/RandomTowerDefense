using System;

public class UnitFactory : SingletonMonoBase<UnitFactory>
{
    public event Action<UnitBase> OnUnitCreat;


    /// <summary>
    /// 종류가 정해진 유닛을 생성해 주는 함수
    /// </summary>
    /// <typeparam name="T">초월체 BeyondBase , 그외 UniiBase</typeparam>
    /// <param name="slot">유닛에게 할당할 슬롯</param>
    /// <param name="kind">유닛에게 할당할 종류</param>
    /// <param name="rank">유닛에게 할당할 랭크</param>
    public T UnitCreat<T>(Slot slot, UnitKind kind, UnitRank rank)
        where T : UnitBase
    {
        if (slot == null) return null;

        string poolKey = rank == UnitRank.Beyond ? $"Beyond_{kind}" : kind.ToString();

        UnitBase unit = ObjectPoolManager.Instance.Get(poolKey)
                                                  .Get()
                                                  .GetComponent<T>()
                                                  .UnitSet(slot, kind, rank);

        if ((int)unit.Rank >= (int)UnitRank.Unique)
        {
            SoundManager.Instance.Vibrate();
            SoundManager.Instance.PlaySound(SFX.HighUnitSpawn);
        }

        OnUnitCreat?.Invoke(unit);
        return (T)unit;
    }

    /// <summary>
    /// 랜덤유닛을 생성해 주는 함수 , 랜덤종류가 아닌 경우 kind를 입력해 오버로드 함수사용
    /// </summary>
    /// <typeparam name="T">초월체 BeyondBase , 그외 UniiBase</typeparam>
    /// <param name="slot">유닛에게 할당할 슬롯</param>
    /// <param name="rank">유닛에게 할당할 랭크</param>
    public T UnitCreat<T>(Slot slot, UnitRank rank)
        where T : UnitBase
    {
        if (slot == null) return null;

        UnitKind kind = RandomUnitKind();

        string poolKey = rank == UnitRank.Beyond ? $"Beyond_{kind}" : kind.ToString();

        UnitBase unit = ObjectPoolManager.Instance.Get(poolKey)
                                                  .Get()
                                                  .GetComponent<T>()
                                                  .UnitSet(slot, kind, rank);

        if ((int)unit.Rank >= (int)UnitRank.Unique)
        {
            SoundManager.Instance.Vibrate();
            SoundManager.Instance.PlaySound(SFX.HighUnitSpawn);
        }

        OnUnitCreat?.Invoke(unit);
        return (T)unit;
    }

    /// <summary>
    /// 랜덤한 유닛 종류를 반환해주는 함수
    /// </summary>
    /// <returns>랜덤 유닛 종류</returns>
    public UnitKind RandomUnitKind()
    {
        return (UnitKind)UnityEngine.Random.Range(0, Enum.GetValues(typeof(UnitKind)).Length);
    }
}
