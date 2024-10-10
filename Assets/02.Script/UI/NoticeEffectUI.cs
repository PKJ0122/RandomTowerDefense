using System;
using UnityEngine;


public class NoticeEffectUI : UIBase
{
    ItemDatas _itemDatas;
    ItemEffect _itemEffectPrefab;
    Transform _itemEffectLocation;


    protected override void Awake()
    {
        base.Awake();
        _itemDatas = Resources.Load<ItemDatas>("ItemDatas");
        _itemEffectPrefab = _itemDatas.itemEffect;
        _itemEffectLocation = transform.Find("Panel/Scroll View/Viewport/Content").GetComponent<Transform>();
        ObjectPoolManager.Instance.CreatePool("itemEffectPrefab", _itemEffectPrefab, 8);

        BeyondCraftingCounterManager.Instance.OnCounterCreate += conter =>
        {
            conter.OnBeyondCraftingPossibleChange += v =>
            {
                if (v)
                {
                    Sprite unitImg = UnitRepository.UnitKindDatas[conter.Method.unitKind].unitImg;
                    Show(unitImg, "\"초월\" 준비완료");
                }
            };
        };
    }

    private void Start()
    {
        UIManager.Instance.Get<MissionUI>().OnMissionClear += missionBase =>
        {
            Show(null, $"\"{missionBase.missionName}\" 미션 클리어");
        };
        UIManager.Instance.Get<UnitSelectSpawnUI>().OnIsUse += unit =>
        {
            Sprite unitImg = UnitRepository.UnitKindDatas[unit.Kind].unitImg;
            Show(unitImg, "소환완료.");
        };
    }

    /// <summary>
    /// 아이템 사용상황,미션 클리어 현황을 알려주는 UI
    /// </summary>
    /// <param name="itemImage">이미지가 없는 경우 null</param>
    public void Show(Sprite itemImage, string detail)
    {
        ItemEffect obj = ObjectPoolManager.Instance.Get("itemEffectPrefab")
                                                   .Get()
                                                   .GetComponent<ItemEffect>()
                                                   .Denote(itemImage, detail);
        obj.transform.SetParent(_itemEffectLocation, false);
        obj.transform.SetAsLastSibling();
    }
}