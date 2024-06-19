using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �����Ͱ� ����� / ���ҽý��� �ִ� ��ũ���ͺ� ������Ʈ �ε��Ͽ� ���
/// </summary>
public class UnitRepository : MonoBehaviour
{
    static UnitDatas _unitDatas;
    static public UnitDatas UnitDatas
    {
        get
        {
            if (_unitDatas == null)
                _unitDatas = Resources.Load<UnitDatas>("UnitDatas");

            return _unitDatas;
        }
    }

    static Dictionary<UnitKind, UnitData> s_unitKindDatas;
    static public Dictionary<UnitKind, UnitData> UnitKindDatas
    {
        get
        {
            if (s_unitKindDatas == null)
            {
                s_unitKindDatas = new Dictionary<UnitKind, UnitData>();
                foreach (UnitData item in UnitDatas.unitDatas)
                {
                    s_unitKindDatas.Add(item.unitKind, item);
                    ObjectPoolManager.Instance.CreatePool($"{item.unitKind}", item.unitObject);
                }
            }
            return s_unitKindDatas;
        }
    }

    static Dictionary<UnitRank, UnitRankData> s_unitRankDatas;
    static public Dictionary<UnitRank, UnitRankData> UnitRankDatas
    {
        get
        {
            if (s_unitRankDatas == null)
            {
                s_unitRankDatas = new Dictionary<UnitRank, UnitRankData>();
                foreach (UnitRankData item in UnitDatas.unitRankColorDatas)
                {
                    s_unitRankDatas.Add(item.unitRank, item);
                }
            }
            return s_unitRankDatas;
        }
    }


    private void Awake()
    {
        var unitKindDatas = UnitKindDatas;
    }
}
