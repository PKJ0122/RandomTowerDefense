using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유닛 데이터가 저장소 / 리소시스에 있는 스크립터블 오브젝트 로드하여 사용
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
