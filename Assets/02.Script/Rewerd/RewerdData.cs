using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RewerdData", menuName = "ScriptableObject/RewerdData")]
[Serializable]
public class RewerdData : ScriptableObject
{
    public RewerdRank rewerdRank;
    public GameObject rewerPrefab;
    public RewerdPercentageData[] rewerdPercentageDatas;
}