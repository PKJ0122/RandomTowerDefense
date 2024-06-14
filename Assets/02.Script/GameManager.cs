using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBase<GameManager>
{
    const float ROUND_TIME = 60f;

    int _wave = -1;
    public int Wave 
    { 
        get => _wave; 
        set
        {
            _wave = value;
            onWaveChange?.Invoke(value);
        } 
    }

    float _time;
    public float Time
    {
        get => Time;
        set
        {
            _time = value;
            onTimeChange?.Invoke(value);
        }
    }

    public event Action<int> onWaveChange;
    public event Action<float> onTimeChange;


    protected override void Awake()
    {
        base.Awake();
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif
    }

    YieldInstruction delay = new WaitForSeconds(1f); //딜레이로 사용할 객체

    /// <summary>
    /// 게임로직 코루틴
    /// </summary>
    IEnumerator C_Game()
    {
        Wave = 0;

        while (Wave+1 <= 50)
        {
            Time = ROUND_TIME;

            while (Time > 0)
            {
                yield return delay;
                Time--;
            }

            Wave++;
        }

        GameClear();
    }

    /// <summary>
    /// 게임 클리어 함수
    /// </summary>
    void GameClear()
    {

    }

    /// <summary>
    /// 게임 오버 함수
    /// </summary>
    public void GameOver()
    {

    }
}
