using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonEnforceUI : UIBase
{

    const float DELAY = 0.1f;

    LayerMask _layerMank;

    bool _IsMoveing;
    float _tick;

    DragonBase _currentDragon;


    protected override void Awake()
    {
        base.Awake();
        _layerMank = LayerMask.GetMask("Slot");
    }


    void Update()
    {
        if (!_canvas.enabled) return;

        if (_IsMoveing)
        {
            _tick += Time.deltaTime;
            if (_tick >= DELAY) Hide();

            return;
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _layerMank))
                {
                    UnitBase unit = SlotManager.Slots[hit.collider.GetComponent<Slot>()];
                    EnforceMaterial material = _currentDragon.EnforceMaterial.Value;
                    if (material.unitKind == unit.Kind && material.unitRank == unit.Rank)
                    {
                        _currentDragon.Enforce++;   
                    }
                    else
                    {
                        Hide();
                    }
                }
                else
                {
                    Hide();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _layerMank))
            {
                UnitBase unit = SlotManager.Slots[hit.collider.GetComponent<Slot>()];
                EnforceMaterial material = _currentDragon.EnforceMaterial.Value;
                if (material.unitKind == unit.Kind && material.unitRank == unit.Rank)
                {
                    _currentDragon.Enforce++;
                }
                else
                {
                    Hide();
                }
            }
            else
            {
                Hide();
            }
        }
    }

    public void Show(DragonBase dragon)
    {
        base.Show();
        _currentDragon = dragon;
    }

    public override void Hide()
    {
        base.Hide();
        _currentDragon = null;
    }
}
