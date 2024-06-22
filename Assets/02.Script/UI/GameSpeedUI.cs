using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedUI : UIBase
{
    const float MAX_SPEED = 3f;
    const float MIN_SPEED = 1f;

    Button _gameSpeedB;
    TMP_Text _gameSpeedT;

    float _gameSpeed;
    public float GameSpeed
    {
        get => _gameSpeed;
        set
        {
            _gameSpeed = value > MAX_SPEED ? MIN_SPEED : value;
            _gameSpeedT.text = $"{_gameSpeed}¹è¼Ó";
            Time.timeScale = _gameSpeed;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _gameSpeed = MIN_SPEED;
        _gameSpeedB = transform.Find("Button - MissionButton").GetComponent<Button>();
        _gameSpeedT = transform.Find("Button - MissionButton/Panel/Text (TMP) - GameSpeed").GetComponent<TMP_Text>();

        _gameSpeedB.onClick.AddListener(() => GameSpeed++);
    }
}
