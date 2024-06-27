using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionDatas", menuName = "ScriptableObject/MissionDatas")]
[Serializable]
public class MissionDatas : ScriptableObject
{
    public GameObject missionPrefeb;
    public MissionBase[] missionDatas;
}
