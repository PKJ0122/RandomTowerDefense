using UnityEngine;

[CreateAssetMenu(fileName = "AuspiciousStart", menuName = "ScriptableObject/Item/AuspiciousStart")]
public class AuspiciousStart : ItemBase
{
    protected override void Use()
    {
        GameManager.Instance.OnWaveChange += v =>
        {
            if (v == 0)
            {
                GameManager.Instance.Gold += (int)Value;
            }
        };
    }
}