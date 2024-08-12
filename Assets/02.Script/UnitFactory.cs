using System;

public class UnitFactory : SingletonMonoBase<UnitFactory>
{
    public event Action<UnitBase> OnUnitCreat;


    /// <summary>
    /// ������ ������ ������ ������ �ִ� �Լ�
    /// </summary>
    /// <typeparam name="T">�ʿ�ü BeyondBase , �׿� UniiBase</typeparam>
    /// <param name="slot">���ֿ��� �Ҵ��� ����</param>
    /// <param name="kind">���ֿ��� �Ҵ��� ����</param>
    /// <param name="rank">���ֿ��� �Ҵ��� ��ũ</param>
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
    /// ���������� ������ �ִ� �Լ� , ���������� �ƴ� ��� kind�� �Է��� �����ε� �Լ����
    /// </summary>
    /// <typeparam name="T">�ʿ�ü BeyondBase , �׿� UniiBase</typeparam>
    /// <param name="slot">���ֿ��� �Ҵ��� ����</param>
    /// <param name="rank">���ֿ��� �Ҵ��� ��ũ</param>
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
    /// ������ ���� ������ ��ȯ���ִ� �Լ�
    /// </summary>
    /// <returns>���� ���� ����</returns>
    public UnitKind RandomUnitKind()
    {
        return (UnitKind)UnityEngine.Random.Range(0, Enum.GetValues(typeof(UnitKind)).Length);
    }
}
