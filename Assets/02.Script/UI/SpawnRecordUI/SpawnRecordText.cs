using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnRecordText : PoolObject
{
    TMP_Text _text;


    void Awake()
    {
        _text = transform.Find("Text (TMP) - SpawnRecord").GetComponent<TMP_Text>();
    }

    public SpawnRecordText SetText(UnitBase unit)
    {
        UnitRankData unitRankData = UnitRepository.UnitRankDatas[unit.Rank];
        string rankName = unitRankData.unitRankName;
        string rankColor = ColorUtility.ToHtmlStringRGB(unitRankData.unitRankColor);

        string unitName = UnitRepository.UnitKindDatas[unit.Kind].unitName;

        _text.text = $"<size=30><b><color=#{rankColor}>{rankName}</b></color=#{rankColor}></size=30> 등급의 <size=30><b>{unitName}</b></size=30>이/가 소환되었습니다.";

        StartCoroutine(C_Activity());

        return this;
    }

    YieldInstruction _delay = new WaitForSeconds(5);

    IEnumerator C_Activity()
    {
        yield return _delay;

        RelasePool();
    }
}
