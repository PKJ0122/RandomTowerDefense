using UnityEngine;

[CreateAssetMenu(fileName = "BeyondOwner", menuName = "ScriptableObject/Mission/BeyondOwner")]
public class BeyondOwner : MissionBase
{
    public override void Init()
    {
        base.Init();
        UIManager.Instance.Get<BeyondCraftingUI>().OnBeyond += (unit) =>
        {
            Progress++;
        };
    }
}