using UnityEngine;

[CreateAssetMenu(fileName = "Promotion", menuName = "ScriptableObject/Item/Promotion")]
public class Promotion : ItemBase
{
    protected override void Use()
    {
        GameManager.Instance.Salary += (int)Value;
        Notice();
    }
}
