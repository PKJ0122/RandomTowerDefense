using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBase<GameManager>
{
    int _wave = -1;
    public int Wave 
    { 
        get => _wave; 
        set
        {
            _wave = value;
            OnChangedWave?.Invoke(value);
        } 
    }

    public Action<int> OnChangedWave;


    protected override void Awake()
    {
        base.Awake();
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif
    }

    void Start()
    {
        Wave = 0;
    }
}
