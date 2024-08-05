using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemEffect : PoolObject
{
    Image _item;
    TMP_Text _itemName;


    void Awake()
    {
        _item = transform.Find("Image - Item").GetComponent<Image>();
        _itemName = transform.Find("Text (TMP) - ItemName").GetComponent<TMP_Text>();
    }

    /// <summary>
    /// 아이템 사용상황,미션 클리어 현황을 알려주는 UI
    /// </summary>
    /// <param name="item">아이템 이미지, 미션인 경우 null</param>
    public ItemEffect Denote(Sprite item, string detail)
    {
        _item.gameObject.SetActive(item != null);
        _item.sprite = item;
        _itemName.text = detail;
        StartCoroutine(C_Activity());

        return this;
    }

    WaitForSecondsRealtime _delay = new WaitForSecondsRealtime(2f);

    IEnumerator C_Activity()
    {
        yield return _delay;

        RelasePool();
    }
}