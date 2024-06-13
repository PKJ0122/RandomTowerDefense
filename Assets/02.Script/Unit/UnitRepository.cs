using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �����Ͱ� ����� / ���ҽý��� �ִ� ��ũ���ͺ� ������Ʈ �ε��Ͽ� ���
/// </summary>
public class UnitRepository : MonoBehaviour
{
    static UnitData _unitData;
    static public UnitData UnitData
    {
        get
        {
            if (_unitData == null)
                _unitData = Resources.Load<UnitData>("UnitData");

            return _unitData;
        }
    }

    static Dictionary<(UnitKind kind,UnitRank rank), float> s_unitPowerDatas;
    static public Dictionary<(UnitKind kind,UnitRank rank), float> UnitPowerDatas
    {
        get
        {
            if (s_unitPowerDatas == null)
            {
                s_unitPowerDatas = new Dictionary<(UnitKind kind, UnitRank rank), float>();
                foreach (UnitPowerData item in UnitData.unitPowerDatas)
                {
                    for (int i = 0; i < item.unitPowerDatas.Length; i++)
                    {
                        s_unitPowerDatas.Add((item.unitKind, (UnitRank)i), item.unitPowerDatas[i]);
                    }
                }
            }
            return s_unitPowerDatas;
        }
    }

    static Dictionary<UnitRank, Color> s_unitRankColorDatas;
    static public Dictionary<UnitRank, Color> UnitRankColorDatas
    {
        get
        {
            if (s_unitRankColorDatas == null)
            {
                s_unitRankColorDatas = new Dictionary<UnitRank, Color>();
                foreach (UnitRankColorData item in UnitData.unitRankColorDatas)
                {
                    s_unitRankColorDatas.Add(item.unitRank, item.unitRankColorData);
                }
            }
            return s_unitRankColorDatas;
        }
    }
}
