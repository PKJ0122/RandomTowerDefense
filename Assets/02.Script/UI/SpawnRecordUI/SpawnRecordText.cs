using System.Collections;
using TMPro;
using UnityEngine;

public class SpawnRecordText : PoolObject
{
    TMP_Text _text;


    protected override void Awake()
    {
        base.Awake();
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

    WaitForSecondsRealtime _delay = new WaitForSecondsRealtime(3f);

    IEnumerator C_Activity()
    {
        yield return _delay;

        RelasePool();
    }
}
