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
    public ItemEffect Denote(Sprite item, string itemName)
    {
        _item.gameObject.SetActive(item != null);
        _item.sprite = item;
        _itemName.text = item == null ? $"\"{itemName}\" 클리어" : $"{itemName} 효과 발동";
        StartCoroutine(C_Activity());

        return this;
    }

    YieldInstruction _delay = new WaitForSeconds(2f);

    IEnumerator C_Activity()
    {
        yield return _delay;

        RelasePool();
    }
}