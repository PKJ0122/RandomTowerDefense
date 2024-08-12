using System;
using UnityEngine;
using UnityEngine.UI;

public class BeyondCraftingShopUI : UIBase
{
    Button _beyondCraftingSlotPrefab;
    BeyondCraftingMethods _beyondCraftingMethods;
    Transform _slotLocation;

    Button _close;

    event Action OnBreak;


    protected override void Awake()
    {
        base.Awake();
        _beyondCraftingSlotPrefab = Resources.Load<Button>("Button - BeyondCraftingShopSlot");
        _beyondCraftingMethods = Resources.Load<BeyondCraftingMethods>("BeyondCraftingMethods");
        _slotLocation = transform.Find("Panel/Image/Scroll View/Viewport/Content").GetComponent<Transform>();
        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _close.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _close.onClick.AddListener(Hide);
        foreach (BeyondCraftingMethod item in _beyondCraftingMethods.beyondCraftingMethods)
        {
            if (!PlayerData.BeyondCraftingDatas.ContainsKey(item.unitKind))
            {
                BeyondCraftingData newBeyondCraftingData = new BeyondCraftingData()
                {
                    unitKind = item.unitKind,
                    IsHave = false
                };
                PlayerData.BeyondCraftingDatas.Add(item.unitKind, newBeyondCraftingData);
                PlayerData.PlayerDataContainer.beyondCraftingDatas.Add(newBeyondCraftingData);
            }

            BeyondCraftingData beyondCraftingData = PlayerData.BeyondCraftingDatas[item.unitKind];
            Button slot = Instantiate(_beyondCraftingSlotPrefab, _slotLocation);
            slot.onClick.AddListener(() => UIManager.Instance.Get<BeyondCraftingInfoUI>().Show(item, beyondCraftingData));
            slot.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
            Image unitImg = slot.transform.Find("Image - Unit").GetComponent<Image>();
            unitImg.sprite = UnitRepository.UnitKindDatas[item.unitKind].unitImg;
            unitImg.color = beyondCraftingData.IsHave ? Color.white : Color.black;
            
            void HaveChange(bool value)
            {
                if (value) unitImg.color = Color.white;
            }

            beyondCraftingData.OnIsHaveChange += HaveChange;
            OnBreak += () => { beyondCraftingData.OnIsHaveChange -= HaveChange; };
        }
    }

    private void OnDisable()
    {
        OnBreak?.Invoke();
    }
}
