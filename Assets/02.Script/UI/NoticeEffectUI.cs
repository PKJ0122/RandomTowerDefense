using System;
using UnityEngine;


public class NoticeEffectUI : UIBase
{
    ItemDatas _itemDatas;
    ItemEffect _itemEffectPrefab;
    Transform _itemEffectLocation;

    Action OnBreak;


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
                    Show(unitImg, "\"�ʿ�\" �غ�Ϸ�");
                }
            };
        };

        foreach (var item in _itemDatas.Items)
        {
            void onItemUse(ItemBase itemBase)
            {
                Show(item.Value.itemImage, $"\"{item.Value.itemName}\" ȿ���ߵ�");
            }

            item.Value.OnItemUse += onItemUse;
            OnBreak += () =>
            {
                item.Value.OnItemUse -= onItemUse;
            };
        }
    }

    private void Start()
    {
        UIManager.Instance.Get<MissionUI>().OnMissionClear += missionBase =>
        {
            Show(null, $"\"{missionBase.missionName}\" �̼� Ŭ����");
        };
        UIManager.Instance.Get<UnitSelectSpawnUI>().OnIsUse += unit =>
        {
            Sprite unitImg = UnitRepository.UnitKindDatas[unit.Kind].unitImg;
            Show(unitImg, "��ȯ�Ϸ�.");
        };
    }

    private void OnDestroy()
    {
        OnBreak?.Invoke();
    }


    /// <summary>
    /// ������ ����Ȳ,�̼� Ŭ���� ��Ȳ�� �˷��ִ� UI
    /// </summary>
    /// <param name="itemImage">�̹����� ���� ��� null</param>
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