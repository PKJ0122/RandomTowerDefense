using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionData", menuName = "ScriptableObject/MissionData")]
[Serializable]
public class MissionData : ScriptableObject
{
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

    bool isClear;
    public bool IsClear
    {
        get => isClear;
        set
        {
            isClear = value;
            onIsClearChange?.Invoke(value);
        }
    }

    public event Action<int> onProgressChange;
    public event Action<bool> onIsClearChange;


    public virtual void Init()
    {
        Progress = 0;
        isClear = false;
    }
}