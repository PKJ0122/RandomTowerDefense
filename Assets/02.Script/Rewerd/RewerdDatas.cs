using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RewerdDatas", menuName = "ScriptableObject/RewerdDatas")]
[Serializable]
public class RewerdDatas : ScriptableObject
{
    public RewerdData[] rewerdDatas;
}