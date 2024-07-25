using System;
using UnityEngine;

[Serializable]
public abstract class QuestBase : ScriptableObject
{
    public string questName;
    public QuestCategory category;
    public int reward;
    public int requirements;

    public abstract void Init();
}