using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemDrawUI : UIBase
{
    ItemDatas _itemDatas;
    Transform[] _itemSlots = new Transform[5];
    Image[] _itemImgs = new Image[5];
    Button _skip;

    Coroutine _drawCoroutine;
    string _drawItemName;


    protected override void Awake()
    {
        base.Awake();
        _itemDatas = Resources.Load<ItemDatas>("ItemDatas");
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            _itemSlots[i] = transform.Find($"Panel/Image/GameObject - ItemSlot/Image - Slot{i}");
        }
        for (int i = 0; i < _itemImgs.Length; i++)
        {
            _itemImgs[i] = _itemSlots[i].Find("Image").GetComponent<Image>();
        }
        _skip = transform.Find("Panel/Image/Button - Skip").GetComponent<Button>();
    }

    private void Start()
    {
        _skip.onClick.AddListener(() =>
        {
            if (_drawCoroutine == null) return;

            foreach (Transform item in _itemSlots) item.localScale = Vector3.one;

            StopCoroutine(_drawCoroutine);
            Hide();
            UIManager.Instance.Get<ItemInfoUI>().Show(_drawItemName);
        });
    }

    public override void Show()
    {
        base.Show();
        _drawCoroutine = null;
        _drawItemName = string.Empty;
        SortingOrder = 100;
        _skip.interactable = false;
        _drawCoroutine = StartCoroutine(C_ItemDraw());
    }

    public override void Hide()
    {
        base.Hide();
    }

    YieldInstruction _delay01 = new WaitForSeconds(0.1f);
    YieldInstruction _delay05 = new WaitForSeconds(0.5f);
    IEnumerator C_ItemDraw()
    {
        int drawItemNum = Random.Range(0, 5);
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            int randomItem = Random.Range(0, _itemDatas.itemDatas.Length);
            _itemImgs[i].sprite = _itemDatas.itemDatas[randomItem].itemImage;

            if (i == drawItemNum)
            {
                _drawItemName = _itemDatas.itemDatas[randomItem].itemName;
                PlayerData.Instance.SetItemAmount(_drawItemName, 1);
            }
        }

        _skip.interactable = true;

        int tick = 3;
        _itemSlots[tick].localScale = new Vector3(1.2f, 1.2f, 1.2f);
        while (tick <= 33)
        {
            yield return _delay01;
            _itemSlots[tick++ % 5].localScale = Vector3.one;
            _itemSlots[tick % 5].localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        for (int i = tick; i <= tick + drawItemNum; i++)
        {
            yield return _delay05;
            _itemSlots[i % 5].localScale = Vector3.one;
            _itemSlots[(i + 1) % 5].localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
        yield return _delay05;
        yield return _delay05;
        _itemSlots[drawItemNum].localScale = Vector3.one;
        Hide();
        UIManager.Instance.Get<ItemInfoUI>().Show(_drawItemName);
    }
}
