using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Negotiation", menuName = "ScriptableObject/Item/Negotiation")]
public class Negotiation : ItemBase
{
    protected override void Use()
    {
        UIManager.Instance.Get<UnitInfoUI>().OnUnitMix += unit =>
        {
            float random = Random.Range(0f, 100f);

            if (0 <= random && random < Value)
            {
                UnitRank unitrank = (UnitRank)((int)unit.Rank - 1);

                Slot slot = null;
                foreach (KeyValuePair<Slot, UnitBase> item in SlotManager.Slots)
                {
                    if (item.Value == null)
                    {
                        slot = item.Key;
                        break;
                    }
                }

                UnitFactory.Instance.UnitCreat<UnitBase>(slot, unitrank);
                Notice();
            }
        };
    }
}
