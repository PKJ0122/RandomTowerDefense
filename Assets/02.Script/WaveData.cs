using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObject/WaveData")]
public class WaveData : ScriptableObject
{
    [SerializeField] public float[] HpDatas = new float[40];
}
