using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShopPageBase : MonoBehaviour
{
    protected const int GOLD = 22;
    protected const int DIAMOND = 23;

    protected Transform _location;
    public Transform Location { get => _location; }

    protected Transform[] _slots;
    public Transform[] Slots { get => _slots; }


    protected string _toggleObjectName;


    protected virtual void Awake()
    {
        _location = transform.Find("GameObject - Slots").GetComponent<Transform>();
        _slots = new Transform[Location.childCount];
        for (int i = 0; i < Location.childCount; i++)
        {
            _slots[i] = _location.Find($"Image - ShopSlot{i}").GetComponent<Transform>();
        }
    }

    public virtual void SetPage(Dictionary<string, ShopPageBase> shopPages)
    {
        if (_toggleObjectName == "") Debug.LogError("딕셔너리의 키값인 string값이 초기화 되지 않았습니다..");
        if (!shopPages.TryAdd(_toggleObjectName, this)) Debug.LogError("이미 등록된 상점 페이지 입니다.");
        ShopRefresh();
    }

    protected abstract void ShopRefresh();
}
