using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDatas", menuName = "ScriptableObject/QuestDatas")]
[Serializable]
public class QuestDatas : ScriptableObject
{
    Dictionary<string, QuestBase> _quests;
    public Dictionary<string, QuestBase> Quests
    {
        get
        {
            if (_quests == null)
            {
                _quests = new Dictionary<string, QuestBase>();
                foreach (QuestBase item in questDatas)
                {
                    _quests.Add(item.questName, item);
                }
            }
            return _quests;
        }
    }

    public QuestBase[] questDatas;
}
