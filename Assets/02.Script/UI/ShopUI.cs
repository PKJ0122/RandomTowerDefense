using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : UIBase
{
    Dictionary<string,ShopPageBase> _shopPageBase = new Dictionary<string,ShopPageBase>();

    Toggle[] _shops;

    Button _close;


    protected override void Awake()
    {
        base.Awake();
        Transform _toggleLocation = transform.Find("Panel/Image/GameObject - Toggle");
        _shops = new Toggle[_toggleLocation.childCount];
        _shops = _toggleLocation.GetComponentsInChildren<Toggle>();
        foreach (Toggle _shop in _shops)
        {
            _shop.onValueChanged.AddListener(value =>
            {
                _shop.transform.localScale = value ? new Vector3(1.3f, 1.3f, 1.3f) : Vector3.one;
                _toggleLocation.GetComponent<VerticalLayoutGroup>().SetLayoutVertical();
                _shopPageBase[_shop.gameObject.name].transform.SetAsLastSibling();
                SoundManager.Instance.PlaySound(SFX.Button_Click);
            });
        }
        Transform _shopLocation = transform.Find("Panel/Image/GameObject - Shop");
        ShopPageBase[] _shopPage = _shopLocation.GetComponentsInChildren<ShopPageBase>();

        foreach (ShopPageBase item in _shopPage) item.SetPage(_shopPageBase);
        
        _shops[0].transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        
        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _close.onClick.AddListener(() => SoundManager.Instance.PlaySound(SFX.Button_Click));
        _close.onClick.AddListener(Hide);
    }

    public void Start()
    {
        StartCoroutine(C_NextDayCheck());
    }

    public override void Show()
    {
        base.Show();
        _shops[0].isOn = true;
    }

    YieldInstruction _delay = new WaitForSeconds(1f);

    IEnumerator C_NextDayCheck()
    {
        while (true)
        {
            DateTime lastShopChange = PlayerData.Instance.LastShopChange;
            DateTime nowTime = DateTime.Now;

            if (lastShopChange.Date < nowTime.Date)
            {
                PlayerData.Instance.LastShopChange = nowTime;
            }

            yield return _delay;
        }
    }
}
