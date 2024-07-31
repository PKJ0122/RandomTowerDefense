using UnityEngine;
using UnityEngine.UI;

public class BeyondCraftingShopUI : UIBase
{
    Button _beyondCraftingSlotPrefab;
    BeyondCraftingMethods _beyondCraftingMethods;
    Transform _slotLocation;

    Button _close;


    protected override void Awake()
    {
        base.Awake();
        _beyondCraftingSlotPrefab = Resources.Load<Button>("Button - BeyondCraftingShopSlot");
        _beyondCraftingMethods = Resources.Load<BeyondCraftingMethods>("BeyondCraftingMethods");
        _slotLocation = transform.Find("Panel/Image/Scroll View/Viewport/Content").GetComponent<Transform>();
        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _close.onClick.AddListener(Hide);
        foreach (BeyondCraftingMethod item in _beyondCraftingMethods.beyondCraftingMethods)
        {
            if (!PlayerData.beyondCraftingDatas.ContainsKey(item.unitKind))
            {
                BeyondCraftingData newBeyondCraftingData = new BeyondCraftingData()
                {
                    unitKind = item.unitKind,
                    IsHave = false
                };
                PlayerData.Instance.PlayerDataContainer.beyondCraftingDatas.Add(newBeyondCraftingData);
                PlayerData.beyondCraftingDatas.Add(item.unitKind, newBeyondCraftingData);
            }

            BeyondCraftingData beyondCraftingData = PlayerData.beyondCraftingDatas[item.unitKind];
            Button slot = Instantiate(_beyondCraftingSlotPrefab, _slotLocation);
            slot.onClick.AddListener(() => UIManager.Instance.Get<BeyondCraftingInfoUI>().Show(item, beyondCraftingData));
            Image unitImg = slot.transform.Find("Image - Unit").GetComponent<Image>();
            unitImg.sprite = UnitRepository.UnitKindDatas[item.unitKind].unitImg;
            unitImg.color = beyondCraftingData.IsHave ? Color.white : Color.black;
            beyondCraftingData.OnIsHaveChange += v =>
            {
                if (v) unitImg.color = Color.white;
            };
        }
    }
}
