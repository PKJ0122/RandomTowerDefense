using Unity.VisualScripting;
using UnityEngine;

public class UnitMoveUI : UIBase
{
    Slot _currentSlot;

    LayerMask _layerMank;

    protected override void Awake()
    {
        base.Awake();
        _layerMank = LayerMask.GetMask("Slot");
    }

    private void Update()
    {
        if (!_canvas.enabled) return;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _layerMank))
                {
                    Slot slot = hit.collider.GetComponent<Slot>();
                    UnitMove(slot);
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
                Slot slot = hit.collider.GetComponent<Slot>();
                UnitMove(slot);
            }
            else
            {
                Hide();
            }
        }
    }

    public void Show(Slot currentSlot)
    {
        base.Show();
        _currentSlot = currentSlot;
    }

    void UnitMove(Slot moveSlot)
    {
        if (SlotManager.Slots[moveSlot] == null)
            SlotManager.Slots[_currentSlot].transform.position = moveSlot.transform.position;
        else
            (SlotManager.Slots[_currentSlot].transform.position, SlotManager.Slots[moveSlot].transform.position) =
                (SlotManager.Slots[moveSlot].transform.position, SlotManager.Slots[_currentSlot].transform.position);

        (SlotManager.Slots[_currentSlot], SlotManager.Slots[moveSlot]) =
            (SlotManager.Slots[moveSlot], SlotManager.Slots[_currentSlot]);

        Hide();
    }
}