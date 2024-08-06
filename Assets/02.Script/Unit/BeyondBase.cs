using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeyondBase : UnitBase
{
    public const int MAX_ENFORCE = 15;

    int _enforce;
    public int Enforce
    {
        get => _enforce;
        set
        {
            if (_enforce == value) return;

            _enforce = value;
            EnforceUp();
        }
    }

    UnitInfo? _enforceMaterial;
    public UnitInfo? EnforceMaterial
    {
        get => _enforceMaterial;
        set
        {
            _enforceMaterial = value;
        }
    }

    public override float Power
    {
        get => _power + AdditionalATK;
        set
        {
            base.Power = value;
        }
    }

    float _additionalATK;
    public float AdditionalATK
    {
        get => _additionalATK;
        set
        {
            _additionalATK = value;
        }
    }


    public void EnforceUp()
    {
        if (Enforce == MAX_ENFORCE)
        {
            EnforceMaterial = null;
            return;
        }

        if (Enforce != 0)
        {
            AdditionalATK += AddAdditionalATK();
        }

        int randomMaterial = Random.Range(0, System.Enum.GetValues(typeof(UnitKind)).Length);
        int upMaterial = Enforce == 0 ? 0 : Enforce / 3;
        EnforceMaterial = new UnitInfo()
        {
            unitKind = (UnitKind)randomMaterial,
            unitRank = (UnitRank)upMaterial
        };
    }

    public override UnitBase UnitSet(Slot slot, UnitKind kind, UnitRank rank)
    {
        base.UnitSet(slot, kind, rank);
        EnforceUp();

        return this;
    }

    public float AddAdditionalATK()
    {
        if (EnforceMaterial == null) return 0;
        UnitRank unitRank = EnforceMaterial.Value.unitRank;

        return UnitRepository.UnitKindDatas[Kind].unitPowerDatas[(int)unitRank];
    }

    public override void RelasePool()
    {
        Enforce = 0;
        AdditionalATK = 0;
        EnforceMaterial = null;
        base.RelasePool();
    }
}