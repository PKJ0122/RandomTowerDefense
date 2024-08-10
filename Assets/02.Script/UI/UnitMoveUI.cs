using Unity.VisualScripting;
using UnityEngine;

public class UnitMoveUI : UIBase
{
    const float DELAY = 0.1f;

    Slot _currentSlot;

    LayerMask _layerMank;

    bool _IsMoveing;
    float _tick;


    protected override void Awake()
    {
        base.Awake();
        _layerMank = LayerMask.GetMask("Slot");
    }

    private void Update()
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

    public override void Hide()
    {
        base.Hide();
        _IsMoveing = false;
    }

    void UnitMove(Slot moveSlot)
    {
        UnitBase changeUnit = SlotManager.Instance.Slots[moveSlot];

        SlotManager.Instance.Slots[_currentSlot].Slot = moveSlot;
        SlotManager.Instance.Slots[_currentSlot] = null;

        if (changeUnit != null) changeUnit.Slot = _currentSlot;

        _IsMoveing = true;
        _tick = 0;
    }
}