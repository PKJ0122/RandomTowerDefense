using UnityEngine;

[CreateAssetMenu(fileName = "Tip", menuName = "ScriptableObject/Item/Tip")]
public class Tip : ItemBase
{
    protected override void Use()
    {
        UIManager.Instance.Get<MissionUI>().ClearGold += (int)Value;
        UIManager.Instance.Get<MissionUI>().OnMissionClear += () =>
        {
            Notice();
        };
    }
}
