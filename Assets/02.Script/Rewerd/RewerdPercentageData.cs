using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RewerdPercentageData", menuName = "ScriptableObject/RewerdPercentageData")]
[Serializable]
public class RewerdPercentageData : ScriptableObject
{
    public RewerdList rewerdList;

    public float Percentage;
    public int MinAmount;
    public int MaxAmount;
}