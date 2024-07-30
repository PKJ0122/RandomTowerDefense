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
            OnProgressChange?.Invoke(value);
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
            OnIsClearChange?.Invoke(value);
        }
    }

    public event Action<int> OnProgressChange;
    public event Action<bool> OnIsClearChange;


    public virtual void Init()
    {
        Progress = 0;
        _isClear = false;
    }

    public void Break()
    {
        OnProgressChange = null;
        OnIsClearChange = null;
    }
}