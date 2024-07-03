using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DPSUI : UIBase
{
    public Transform location;
    public DPS prefeb;
    

    private void Start()
    {
        UIManager.Instance.Get<UnitSpawnUI>().OnUnitSpawn += unit =>
        {
            DPS go = Instantiate(prefeb, location);
            go._unit = unit;
            go.GetComponent<Image>().sprite = UnitRepository.UnitKindDatas[unit.Kind].unitImg;
            unit.OnDisable += () => Destroy(go);
            go.transform.SetAsFirstSibling();
        };
    }
}
