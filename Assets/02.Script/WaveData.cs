using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObject/WaveData")]
[Serializable]
public class WaveData : ScriptableObject
{
    public float[] HpDatas = new float[40];

    public Enemy[] nomalEnemyPrefab;
    public Boss[] bossEnemyPrefab;
    public Boss goldBossPrefab;
}