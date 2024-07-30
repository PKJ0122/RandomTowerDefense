using UnityEngine;

[CreateAssetMenu(fileName = "HighInterest", menuName = "ScriptableObject/Item/HighInterest")]
public class HighInterest : ItemBase
{
    protected override void Use()
    {
        GameManager.Instance.Interest += (int)Value;
        Notice();
    }
}
