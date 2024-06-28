using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : UIBase
{
    TMP_Text _wave;
    Slider _timeS;
    TMP_Text _timeT;


    protected override void Awake()
    {
        base.Awake();
        _wave = transform.Find("Image/Text (TMP) - WaveText").GetComponent<TMP_Text>();
        _timeS = transform.Find("Image/Slider - Timer").GetComponent<Slider>();
        _timeT = transform.Find("Image/Slider - Timer/Text (TMP) - Timer").GetComponent<TMP_Text>();

        GameManager.Instance.OnWaveChange += value =>
        {
            _wave.text = $"{value + 1} Wave";
        };
        GameManager.Instance.OnTimeChange += value =>
        {
            _timeS.value = value;
            _timeT.text = value == 60f ? "01 : 00" : $"00 : {(int)value}";
        };
    }
}
