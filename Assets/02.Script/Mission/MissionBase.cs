using System;
using UnityEngine;

[Serializable]
public class MissionBase : ScriptableObject
{
    public string missionName;
    public string detail;
    public int condition;

    int _progress;
    public int Progress
    {
        get => _progress;
        set
        {
            _progress = value;
            onProgressChange?.Invoke(value);
        }
    }

    bool _isClear;
    public bool IsClear
    {
        get => _isClear;
        set
        {
            if (_isClear == value) return;

            _isClear = value;
            onIsClearChange?.Invoke(value);
        }
    }

    public event Action<int> onProgressChange;
    public event Action<bool> onIsClearChange;


    public virtual void Init()
    {
        Progress = 0;
        _isClear = false;
    }
}