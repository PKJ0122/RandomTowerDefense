using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObject/WaveData")]
public class WaveDate : ScriptableObject
{
    [SerializeField] public float[] HpDates;
}
