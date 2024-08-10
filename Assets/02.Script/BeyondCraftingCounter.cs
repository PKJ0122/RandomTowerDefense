using System;
using System.Collections.Generic;

public class BeyondCraftingCounter
{
    public BeyondCraftingCounter(BeyondCraftingMethod method)
    {
        _method = method;
        Init();
    }

    BeyondCraftingMethod _method;
    public BeyondCraftingMethod Method => _method;

    List<UnitBase>[] _materials = new List<UnitBase>[3] { new List<UnitBase>(), new List<UnitBase>(), new List<UnitBase>() };
    public List<UnitBase>[] Materials => _materials;

    bool _isBeyondCraftingPossible;
    public bool IsBeyondCraftingPossible
    {
        get => _isBeyondCraftingPossible;
        set
        {
            if (_isBeyondCraftingPossible == value) return;

            _isBeyondCraftingPossible = value;
            OnBeyondCraftingPossibleChange?.Invoke(value);
        }
    }

    public event Action OnMaterialsStateChange;
    public event Action<bool> OnBeyondCraftingPossibleChange;


    void Init()
    {
        OnMaterialsStateChange += () =>
        {
            for (int i = 0; i < _materials.Length; i++)
            {
                if (_materials[i].Count == 0)
                {
                    IsBeyondCraftingPossible = false;
                    return;
                }
            }
            IsBeyondCraftingPossible = true;
        };
        UnitFactory.Instance.OnUnitCreat += unit =>
        {
            for (int i = 0; i < _materials.Length; i++)
            {
                if (unit.Kind == _method.beyondCraftingMaterials[i].unitKind &&
                    unit.Rank == _method.beyondCraftingMaterials[i].unitRank)
                {
                    UnitAdd(unit, _materials[i]);
                    unit.OnDisable += () =>
                    {
                        UnitRemove(unit, _materials[i]);
                    };
                    break;
                }
            }
        };
    }

    void UnitAdd(UnitBase unit, List<UnitBase> list)
    {
        list.Add(unit);
        OnMaterialsStateChange?.Invoke();
    }

    void UnitRemove(UnitBase unit, List<UnitBase> list)
    {
        list.Remove(unit);
        OnMaterialsStateChange?.Invoke();
    }
}